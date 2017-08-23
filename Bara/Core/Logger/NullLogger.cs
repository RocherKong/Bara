using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace Bara.Core.Logger
{
    public class NullLogger : ILogger
    {
        public static NullLogger Instance { get; } = new NullLogger();
        private NullLogger() {

        }
        public IDisposable BeginScope<TState>(TState state)
        {
            return NullScope.Instance;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
          //  throw new NotImplementedException();
        }
    }
}
