using IronLog.File.Loggers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IronLog.File
{
    public static class FileLoggerExtensions
    {
        public static ILoggingBuilder AddFileLogger(this ILoggingBuilder builder, IConfiguration config)
        {
            builder.Services.AddSingleton<ILoggerProvider>(sp =>
            {
                var env = sp.GetService<IHostEnvironment>();
                return new FileLoggerProvider(config, env);
            });
            return builder;
        }
    }
}
