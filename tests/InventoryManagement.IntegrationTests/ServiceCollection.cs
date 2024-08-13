using InventoryManagement.Core.Abstractions;
using InventoryManagement.Infrastructure.Services;
using InventoryManagement.IntegrationTests.Helpers;
using InventoryManagement.Shared.Abstractions.Clock;
using InventoryManagement.Shared.Abstractions.Contexts;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.Shared.Abstractions.Files;
using InventoryManagement.Shared.Infrastructure.Cache;
using InventoryManagement.Shared.Infrastructure.Clock;
using InventoryManagement.Shared.Infrastructure.Encryption;
using InventoryManagement.Shared.Infrastructure.Localization;
using InventoryManagement.Shared.Infrastructure.Serialization;
using InventoryManagement.WebApi.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using AuthManager = InventoryManagement.IntegrationTests.Helpers.AuthManager;

namespace InventoryManagement.IntegrationTests;

public static class ServiceCollection
{
    public static void AddDefaultInjectedServices(this IServiceCollection services)
    {
        services.AddScoped<IContext>(_ => new Context(Guid.Empty));
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IAuthManager, AuthManager>();
        services.AddSingleton<IFileService, FileSystemServiceMock>();
        services.AddInternalMemoryCache();
        services.AddJsonSerialization();
        services.AddSingleton<IClock, Clock>();
        services.AddSingleton<ISalter, Salter>();
        services.AddEncryption();
        services.AddDistributedMemoryCache();
        services.AddLocalizerJson();
        services.AddSingleton(new ClockOptions());
        services.AddScoped<IFileRepositoryService, FileRepositoryService>();
    }

    public static void EnsureDbCreated<T>(this IServiceCollection services) where T : DbContext
    {
        var serviceProvider = services.BuildServiceProvider();

        using var scope = serviceProvider.CreateScope();
        var scopedServices = scope.ServiceProvider;
        var context = scopedServices.GetRequiredService<T>();
        context.Database.EnsureCreated();
    }
}