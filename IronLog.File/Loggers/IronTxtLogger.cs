using IronLog.File.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace IronLog.File.Loggers
{
    public class IronTxtLogger : ILogger
    {
        private readonly FileLoggerOptions _options;
        private readonly string _categoryName;
        public IronTxtLogger(FileLoggerOptions options, string CategoryName)
        {
            _options = options;
            _categoryName = CategoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true; 
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string FileNextPart = "";
            switch (_options.SplitFormat)
            {
                case SplitType.Infinite: FileNextPart = "Infinite"; break;
                case SplitType.Minute: FileNextPart = DateTime.Now.ToString("yyyyMMddHHmm"); break;
                case SplitType.Hourly: FileNextPart = DateTime.Now.ToString("yyyyMMddHH"); break;
                case SplitType.QuarterlyDaily: FileNextPart = DateTime.Now.ToString("yyyyMMdd") + "_" + (DateTime.Now.Hour / 6).ToString(); break;
                case SplitType.HalfDay: FileNextPart = DateTime.Now.ToString("yyyyMMdd") + "_" + (DateTime.Now.Hour / 12).ToString(); break;
                case SplitType.Daily: FileNextPart = DateTime.Now.ToString("yyyyMMdd"); break;
                case SplitType.Weekly: FileNextPart = DateTime.Now.Year + "_" + (DateTime.Now.DayOfYear / 7); break;
                case SplitType.Monthly: FileNextPart = DateTime.Now.Month.ToString(); break;
                default: FileNextPart = "Infinite"; break;
            }

            var _fileName = String.Format(_options.FileNameStatic, FileNextPart) + "." + _options.LoggerType;

            StringBuilder builder = new StringBuilder(_options.Layout);
            builder.Replace("{date}", DateTime.Now.ToString(_options.DateFormat));
            builder.Replace("{level}", logLevel.ToString());
            builder.Replace("{logger}", _categoryName ?? "");
            builder.Replace("{message}", formatter(state, exception) ?? "");
            builder.Replace("{exception}", exception != null ? exception.InnerException?.Message : "");

            WriteMessageToFile(_options.Path, _fileName,  builder.ToString());

        }

        private static void WriteMessageToFile(string Path, string FileName, string message)
        { 
            using (var streamWriter = new StreamWriter($"{ Path }\\{ FileName }", true))
            {
                streamWriter.WriteLineAsync(message).Wait();
                streamWriter.Close();
            }
        }
    }
}
