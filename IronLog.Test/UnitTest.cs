using IronLog.File;
using IronLog.File.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
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
            var factory = new FileLoggerProvider(config, null);

            var logger = factory.CreateLogger("SampleController");
            logger.LogInformation("Sample 1");
            logger.LogDebug("Sample 2");
            logger.LogError("Sample 3");
            logger.LogTrace("Sample 4");
            logger.LogCritical("Sample 5");
        }
    }
}