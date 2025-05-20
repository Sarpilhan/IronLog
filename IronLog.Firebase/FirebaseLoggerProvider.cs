using IronLog.Firebase.Loggers;
using IronLog.Firebase.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace IronLog.Firebase
{
    public class FirebaseLoggerProvider : ILoggerProvider
    {
        private bool isDisposed;
        private readonly IConfiguration _config;

        public FirebaseLoggerProvider(IConfiguration config)
        {
            _config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            var options = new FirebaseLoggerOptions();
            _config.GetSection(FirebaseLoggerOptions.FirebaseLoggerOption).Bind(options);
            return new IronFireLogger(options, categoryName);
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
