using Bara.Abstract.Cache;
using System;
using System.Collections.Generic;
using System.Text;
using Bara.Abstract.Core;
using Bara.Core.Context;
using Microsoft.Extensions.Logging;

namespace Bara.Core.Cache
{
    public class CacheManager : ICacheManager
    {
        private readonly ILogger _logger;
        public CacheManager(ILoggerFactory loggerFactory, IBaraMapper baraMapper)
        {
            this._logger = loggerFactory.CreateLogger<CacheManager>();
            this.BaraMapper = baraMapper;
            RequestQueue = new Queue<RequestContext>();
        }
        public object this[RequestContext context, Type type] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IBaraMapper BaraMapper { get; set; }
        public Queue<RequestContext> RequestQueue { get; set; }

        public void Enqueue(RequestContext context)
        {
            RequestQueue.Enqueue(context);
        }

        public void ClearQueue()
        {
            throw new NotImplementedException();
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

        }

        public void ResetMappedCaches()
        {
            throw new NotImplementedException();
        }

        public void TriggerFlush(RequestContext context)
        {
            throw new NotImplementedException();
        }
    }
}
