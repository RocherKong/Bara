using Bara.Abstract.Session;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Bara.Core.Session
{
    public class DbConnectionSessionStore : IDbConnectionSessionStore
    {
        const String SessionPrefix = "Bara-Local-DbSession-";
        public String SessionName = String.Empty;
        private AsyncLocal<IDictionary<String, IDbConnectionSession>> StaticSessions = new AsyncLocal<IDictionary<string, IDbConnectionSession>>();
        private readonly ILogger _logger;
        public DbConnectionSessionStore(ILoggerFactory loggerFactory, String BaraMapperHashCode)
        {
            _logger = loggerFactory.CreateLogger<DbConnectionSessionStore>();
            _logger.LogDebug(BaraMapperHashCode);

            SessionName = SessionPrefix + BaraMapperHashCode;
        }

        public IDbConnectionSession LocalSession
        {
            get
            {
                if (StaticSessions.Value == null)
                {
                    return null;
                }
                else
                {
                    StaticSessions.Value.TryGetValue(SessionName, out IDbConnectionSession session);
                    return session;
                }
            }
        }

        public void Dispose()
        {
            if (StaticSessions.Value != null)
            {
                StaticSessions.Value[SessionName] = null;
            }
        }

        public void Store(IDbConnectionSession session)
        {
            if (StaticSessions.Value == null)
            {
                StaticSessions.Value = new Dictionary<String, IDbConnectionSession>();
            }
            StaticSessions.Value[SessionName] = session;
        }
    }
}
