using IronLog.File.Loggers;
using IronLog.File.Model;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace IronLog.File
{
    public class FileLoggerProvider : ILoggerProvider
    {
        private IConfiguration _config;
        public FileLoggerProvider(IConfiguration config)
        {
            _config = config;
        }
        public ILogger CreateLogger(string categoryName)
        {
            FileLoggerOptions options = new FileLoggerOptions(); 
            _config.GetSection(FileLoggerOptions.FileLoggerOption).Bind(options);

            if (string.IsNullOrEmpty(options.Path)) 
                options.Path = _config.GetValue<string>(WebHostDefaults.ContentRootKey); 
             
            switch (options.LoggerType)
            {
                case "txt": return new IronTxtLogger(options, categoryName);
                case "json": return new IronJsonLogger(options, categoryName);
                case "xml": return new IronXmlLogger(options, categoryName);
                default: return null; 
            }
            
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
