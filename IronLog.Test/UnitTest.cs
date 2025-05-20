using IronLog.File;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IronLog.Test
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void MainTest()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var services = new ServiceCollection();
            services.AddLogging(builder => builder.AddFileLogger(config));

            using var provider = services.BuildServiceProvider();
            var logger = provider.GetRequiredService<ILogger<UnitTest>>();
            logger.LogInformation("Sample 1");
            logger.LogDebug("Sample 2");
            logger.LogError("Sample 3");
            logger.LogTrace("Sample 4");
            logger.LogCritical("Sample 5");
        }
    }
}
