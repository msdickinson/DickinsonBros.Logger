using DickinsonBros.Logger.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace DickinsonBros.Logger.Extensions
{

    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggingService(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(typeof(ILoggingService<>), typeof(LoggingService<>));
            serviceCollection.AddSingleton<ICorrelationService, CorrelationService>();

            return serviceCollection;
        }
    }

}
