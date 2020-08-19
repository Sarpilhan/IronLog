using IronLog.File.Loggers;
using IronLog.File.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace IronLog.File
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private bool isDisposed;
        private IConfiguration _config;
        public FileLoggerProvider(IConfiguration config)
        {
            _config = config;
        }
        public ILogger CreateLogger(string categoryName)
        {
            FileLoggerOptions options = new FileLoggerOptions();
            _config.GetSection(FileLoggerOptions.FileLoggerOption).Bind(options);

            if (string.IsNullOrEmpty(options.Path) || options.Path.Substring(0, 1) == "\\")
                options.Path = System.IO.Directory.GetCurrentDirectory() + options.Path;

            switch (options.LoggerType)
            {
                case "txt": return new IronTxtLogger(options, categoryName);
                case "json": return new IronJsonLogger(options, categoryName);
                default: return null;
            }
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
                _config = null;
            }  
            isDisposed = true;
        }
    }
}
