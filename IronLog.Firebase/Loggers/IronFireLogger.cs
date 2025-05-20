using IronLog.Firebase.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace IronLog.Firebase.Loggers
{
    public class IronFireLogger : ILogger
    {
        private readonly FirebaseLoggerOptions _options;
        private readonly string _categoryName;
        private readonly HttpClient _client;
        private readonly JsonSerializerOptions _jsonOptions;

        public IronFireLogger(FirebaseLoggerOptions options, string categoryName)
        {
            _options = options;
            _categoryName = categoryName;
            _client = new HttpClient();
            _jsonOptions = new JsonSerializerOptions { IgnoreNullValues = true };
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                var urlBuilder = new StringBuilder();
                urlBuilder.Append(_options.BaseUrl.TrimEnd('/'));
                urlBuilder.Append('/').Append(_options.LogPath.Trim('/')).Append(".json");
                if (!string.IsNullOrEmpty(_options.AuthToken))
                    urlBuilder.Append("?auth=").Append(_options.AuthToken);

                var obj = new
                {
                    Date = DateTime.Now,
                    Level = logLevel.ToString(),
                    Logger = _categoryName,
                    Message = formatter(state, exception),
                    Exception = exception?.InnerException?.Message
                };

                var content = new StringContent(JsonSerializer.Serialize(obj, _jsonOptions), Encoding.UTF8, "application/json");
                _client.PostAsync(urlBuilder.ToString(), content).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
