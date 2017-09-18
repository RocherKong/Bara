using Bara.Abstract.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Core;
using Bara.Core.Context;
using Microsoft.Extensions.Logging;
using Bara.Model;
using Bara.Exceptions;

namespace Bara.Core.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly ILogger _logger;
        private static readonly object syncobj = new object();
        public CacheManager(ILoggerFactory loggerFactory, IBaraMapper baraMapper)
        {
            this._logger = loggerFactory.CreateLogger<CacheManager>();
            this.BaraMapper = baraMapper;
            RequestQueue = new Queue<RequestContext>();
            MappedLastFlushTimes = new Dictionary<String, DateTime>();
            MappedStatements = BaraMapper.BaraMapConfig.MappedStatements;
        }

        public CacheManager(IBaraMapper baraMapper)
        {
            this.BaraMapper = baraMapper;
            RequestQueue = new Queue<RequestContext>();
            MappedLastFlushTimes = new Dictionary<String, DateTime>();
            MappedStatements = BaraMapper.BaraMapConfig.MappedStatements;
        }

        public IDictionary<String, Statement> MappedStatements { get; }

        public IDictionary<String, DateTime> MappedLastFlushTimes { get; }

        private IDictionary<String, IList<Statement>> _mappedTriggerFlushs;
        public IDictionary<String, IList<Statement>> MappedTriggerFlushs
        {
            get
            {
                if (_mappedTriggerFlushs == null)
                {
                    lock (syncobj)
                    {
                        if (_mappedTriggerFlushs == null)
                        {
                            _logger.LogDebug("CacheManger Start Load MappedTriggerFlushs.");
                            _mappedTriggerFlushs = new Dictionary<String, IList<Statement>>();
                            foreach (var baraMap in BaraMapper.BaraMapConfig.BaraMaps)
                            {
                                foreach (var statement in baraMap.Statements)
                                {
                                    if (statement.Cache == null
                                        || statement.Cache.FlushOnExecutes == null)
                                    {
                                        continue;
                                    }
                                    foreach (var triggerStatement in statement.Cache.FlushOnExecutes)
                                    {
                                        IList<Statement> triggerStatements = null;
                                        if (_mappedTriggerFlushs.ContainsKey(triggerStatement.Statement))
                                        {
                                            triggerStatements = _mappedTriggerFlushs[triggerStatement.Statement];
                                        }
                                        else
                                        {
                                            triggerStatements = new List<Statement>();
                                            _mappedTriggerFlushs[triggerStatement.Statement] = triggerStatements;
                                        }
                                        triggerStatements.Add(statement);
                                    }
                                }
                            }
                            _logger.LogDebug("CacheManger End Load MappedTriggerFlushs.");
                        }
                    }
                }

                return _mappedTriggerFlushs;
            }
        }

        public object this[RequestContext context, Type type]
        {
            get
            {
                string fullSqlId = context.FullSqlId;
                if (!MappedStatements.ContainsKey(fullSqlId))
                {
                    throw new BaraException($"CacheManager can't Load Statement which FullSqlId is {fullSqlId},Please Check!");
                }
                var statement = MappedStatements[fullSqlId];
                if (statement.Cache == null)
                {
                    return null;
                }
                lock (syncobj)
                {
                    FlushByInterval(statement);
                }
                var cacheKey = new CacheKey(context);
                var cache = statement.CacheProvider[cacheKey, type];
                _logger.LogDebug($"CacheManager GetCache From FullSqlId ${fullSqlId},Success:{cache != null}");
                return cache;
            }
            set
            {
                string fullSqlId = context.FullSqlId;
                if (!MappedStatements.ContainsKey(fullSqlId))
                {
                    throw new BaraException($"CacheManager can't Load Statement which FullSqlId is {fullSqlId},Please Check!");
                }
                var statement = MappedStatements[fullSqlId];
                if (statement.Cache == null)
                {
                    return;
                }
                lock (syncobj)
                {
                    FlushByInterval(statement);
                }
                var cacheKey = new CacheKey(context);
                _logger.LogDebug($"CacheManager SetCache FullSqlId:{fullSqlId}");
                statement.CacheProvider[cacheKey, type] = value;
            }
        }

        public IBaraMapper BaraMapper { get; set; }
        public Queue<RequestContext> RequestQueue { get; set; }

        public void Enqueue(RequestContext context)
        {
            RequestQueue.Enqueue(context);
        }

        public void ClearQueue()
        {
            RequestQueue.Clear();
        }

        public void FlushQueue()
        {
            while (RequestQueue.Count > 0)
            {
                Flush(RequestQueue.Dequeue());
            }
        }

        private void Flush(RequestContext context)
        {
            String FullSqlId = context.FullSqlId;
            if (MappedTriggerFlushs.ContainsKey(FullSqlId))
            {
                lock (syncobj)
                {
                    IList<Statement> triggerStatements = MappedTriggerFlushs[FullSqlId];
                    foreach (var statement in triggerStatements)
                    {
                        MappedLastFlushTimes[statement.FullSqlId] = DateTime.Now;
                    }
                }
            }
        }

        public void ResetMappedCaches()
        {
            lock (syncobj)
            {
                _mappedTriggerFlushs = null;
            }
        }

        public void TriggerFlush(RequestContext context)
        {
            var session = BaraMapper.SessionStore.LocalSession;
            if (session != null && session.DbTransaction != null)
            {
                Enqueue(context);
            }
            else
            {
                Flush(context);
            }
        }

        public void FlushByInterval(Statement statement)
        {
            if (statement.Cache.FlushInterval.Interval.Ticks == 0) { return; }
            String FullSqlId = statement.FullSqlId;
            DateTime LastFlushTime = DateTime.Now;
            if (!MappedLastFlushTimes.ContainsKey(FullSqlId))
            {
                MappedLastFlushTimes[FullSqlId] = LastFlushTime;
            }
            else
            {
                LastFlushTime = MappedLastFlushTimes[FullSqlId];
            }
            var lastInterval = DateTime.Now - LastFlushTime;
            if (lastInterval >= statement.Cache.FlushInterval.Interval)
            {
                Flush(statement, lastInterval);
            }
        }

        public void Flush(Statement statement, TimeSpan timeSpan)
        {
            MappedLastFlushTimes[statement.FullSqlId] = DateTime.Now;
            statement.CacheProvider.Flush();
        }
    }
}
