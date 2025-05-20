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
            try
            {
                string FileNextPart = _options.SplitFormat switch
                {
                    SplitType.Infinite => "Infinite",
                    SplitType.Minute => DateTime.Now.ToString("yyyyMMddHHmm"),
                    SplitType.Hourly => DateTime.Now.ToString("yyyyMMddHH"),
                    SplitType.QuarterlyDaily => DateTime.Now.ToString("yyyyMMdd") + "_" + (DateTime.Now.Hour / 6).ToString(),
                    SplitType.HalfDay => DateTime.Now.ToString("yyyyMMdd") + "_" + (DateTime.Now.Hour / 12).ToString(),
                    SplitType.Daily => DateTime.Now.ToString("yyyyMMdd"),
                    SplitType.Weekly => DateTime.Now.Year + "_" + (DateTime.Now.DayOfYear / 7),
                    SplitType.Monthly => DateTime.Now.Month.ToString(),
                    _ => "Infinite",
                };
                var _fileName = String.Format(_options.FileNameStatic, FileNextPart) + "." + _options.LoggerType;
                var obj = new { Date = DateTime.Now, Level = logLevel.ToString(), Logger = _categoryName, Message = formatter(state, exception), Exception = exception?.InnerException?.Message };
                WriteMessageToFile(_options.Path, _fileName, JsonSerializer.Serialize(obj, _jsonOptions));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void WriteMessageToFile(string Path, string FileName, string message)
        {
            if (!Directory.Exists(Path))
                Directory.CreateDirectory(Path);

            var filePath = Path.Combine(Path, FileName);
            using var streamWriter = new StreamWriter(filePath, true);
            streamWriter.WriteLineAsync(message).Wait();
        }
    }
}
