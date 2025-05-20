using IronLog.MySql.Model;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using System;

namespace IronLog.MySql.Loggers
{
    public class IronMySqlLogger : ILogger
    {
        private readonly MySqlLoggerOptions _options;
        private readonly string _categoryName;

        public IronMySqlLogger(MySqlLoggerOptions options, string categoryName)
        {
            _options = options;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => true;

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            try
            {
                using var connection = new MySqlConnection(_options.ConnectionString);
                connection.Open();
                using var command = connection.CreateCommand();
                command.CommandText = $"INSERT INTO {_options.TableName} (Date, Level, Logger, Message, Exception) VALUES (@date, @level, @logger, @message, @exception)";
                command.Parameters.AddWithValue("@date", DateTime.Now);
                command.Parameters.AddWithValue("@level", logLevel.ToString());
                command.Parameters.AddWithValue("@logger", _categoryName);
                command.Parameters.AddWithValue("@message", formatter(state, exception));
                command.Parameters.AddWithValue("@exception", exception?.InnerException?.Message);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
