using InventoryManagement.Shared.Abstractions.Clock;
using InventoryManagement.Shared.Infrastructure.Clock;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagement.Shared.Infrastructure.Localization
{
    public static class ServiceCollection
    {
        public static void AddLocalizerJson(this IServiceCollection services)
        {
            services.AddLocalization();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();
        }
    }
}