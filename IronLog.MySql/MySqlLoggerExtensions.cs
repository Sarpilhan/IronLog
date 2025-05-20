using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IronLog.MySql
{
    public static class MySqlLoggerExtensions
    {
        public static ILoggingBuilder AddMySqlLogger(this ILoggingBuilder builder, IConfiguration config)
        {
            builder.Services.AddSingleton<ILoggerProvider>(sp => new MySqlLoggerProvider(config));
            return builder;
        }
    }
}
