using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace IronLog.Firebase
{
    public static class FirebaseLoggerExtensions
    {
        public static ILoggingBuilder AddFirebaseLogger(this ILoggingBuilder builder, IConfiguration config)
        {
            builder.Services.AddSingleton<ILoggerProvider>(sp => new FirebaseLoggerProvider(config));
            return builder;
        }
    }
}
