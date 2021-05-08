using IronLog.File.Model;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
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

                StringBuilder builder = new StringBuilder(_options.Layout);
                builder.Replace("{date}", DateTime.Now.ToString(_options.DateFormat));
                builder.Replace("{level}", logLevel.ToString());
                builder.Replace("{logger}", _categoryName ?? "");
                builder.Replace("{message}", formatter(state, exception) ?? "");
                builder.Replace("{exception}", exception != null ? exception.InnerException?.Message : "");

                WriteMessageToFile(_options.Path, _fileName, builder.ToString());
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

            using var streamWriter = new StreamWriter($"{ Path }\\{ FileName }", true);
            streamWriter.WriteLineAsync(message).Wait();
            streamWriter.Close();
        }
    }
}