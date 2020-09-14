using DickinsonBros.Logger.Abstractions;
using DickinsonBros.Logger.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DickinsonBros.Logger.Tests.Extensions
{

    [TestClass]
    public class IServiceCollectionExtensionsTests
    {
        [TestMethod]
        public void AddLoggingService_Should_Succeed()
        {
            // Arrange
            var serviceCollection = new ServiceCollection();

            // Act
            serviceCollection.AddLoggingService();

            // Assert
            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(ILoggingService<>) &&
                                                       serviceDefinition.ImplementationType == typeof(LoggingService<>) &&
                                                       serviceDefinition.Lifetime == ServiceLifetime.Singleton));

            Assert.IsTrue(serviceCollection.Any(serviceDefinition => serviceDefinition.ServiceType == typeof(ICorrelationService) &&
                                           serviceDefinition.ImplementationType == typeof(CorrelationService) &&
                                           serviceDefinition.Lifetime == ServiceLifetime.Singleton));

        }
    }

}
