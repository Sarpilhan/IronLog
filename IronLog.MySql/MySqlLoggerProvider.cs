using IronLog.MySql.Loggers;
using IronLog.MySql.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace IronLog.MySql
{
    public class MySqlLoggerProvider : ILoggerProvider
    {
        private bool isDisposed;
        private readonly IConfiguration _config;

        public MySqlLoggerProvider(IConfiguration config)
        {
            _config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var options = new MySqlLoggerOptions();
            _config.GetSection(MySqlLoggerOptions.MySqlLoggerOption).Bind(options);
            return new IronMySqlLogger(options, categoryName);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (isDisposed) return;
            if (disposing)
            {
                // nothing to dispose
            }
            isDisposed = true;
        }
    }
}
