using InventoryManagement.Shared.Abstractions.Clock;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryManagement.Shared.Infrastructure.Clock;

public static class ServiceCollection
{
    public static void AddClock(this IServiceCollection services)
    {
        var clockOptions = services.GetOptions<ClockOptions>("clock");

        services.AddSingleton(clockOptions)
            .AddSingleton<IClock, Clock>();
    }
}