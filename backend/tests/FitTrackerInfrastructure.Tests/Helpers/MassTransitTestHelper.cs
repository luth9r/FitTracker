using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace FitTrackerInfrastructure.Tests.Helpers;

public static class MassTransitTestHelper
{
    public static ServiceProvider CreateConsumerProvider<TConsumer>(params object[] mocks)
        where TConsumer : class, IConsumer
    {
        var services = new ServiceCollection();

        foreach (var mock in mocks)
        {
            var mockType = mock.GetType();

            var interfaceType = mockType.GetInterfaces()
                .FirstOrDefault(i => !i.Namespace?.StartsWith("System") == true
                                     && !i.Namespace?.StartsWith("Moq") == true);

            if (interfaceType != null)
            {
                services.AddSingleton(interfaceType, mock);
            }
        }

        services.AddMassTransitTestHarness(x =>
        {
            x.AddConsumer<TConsumer>();
        });

        return services.BuildServiceProvider(true);
    }
}
