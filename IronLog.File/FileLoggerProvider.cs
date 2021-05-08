using IronLog.File.Loggers;
using IronLog.File.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace IronLog.File
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private bool isDisposed;
        private IConfiguration _config;
        private IHostEnvironment _env;

        public FileLoggerProvider(IConfiguration config, IHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        public ILogger CreateLogger(string categoryName)
        {
            FileLoggerOptions options = new FileLoggerOptions();
            _config.GetSection(FileLoggerOptions.FileLoggerOption).Bind(options);

            if (options.Path.StartsWith("\\"))
            {
                if (_env != null)
                    options.Path = _env.ContentRootPath + options.Path;
                else
                    options.Path = System.IO.Directory.GetCurrentDirectory() + options.Path;
            }

            return options.LoggerType switch
            {
                "txt" => new IronTxtLogger(options, categoryName),
                "json" => new IronJsonLogger(options, categoryName),
                _ => null,
            };
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
                _env = null;
            }
            isDisposed = true;
        }
    }
}