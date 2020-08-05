using IronLog.File.Model;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;

namespace IronLog.File.Loggers
{
    public class IronJsonLogger : ILogger
    {
        private readonly FileLoggerOptions _options;
        private readonly string _categoryName;
        private readonly JsonSerializerOptions _jsonOptions;

        public IronJsonLogger(FileLoggerOptions options, string CategoryName)
        {
            _options = options;
            _categoryName = CategoryName;
            _jsonOptions = new JsonSerializerOptions() { IgnoreNullValues = true };
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
            var obj = new { Date = DateTime.Now, Level = logLevel.ToString(), Logger = _categoryName, Message = formatter(state, exception), Exception = exception?.InnerException?.Message };
            WriteMessageToFile(_options.Path, _fileName, JsonSerializer.Serialize(obj, _jsonOptions));

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
