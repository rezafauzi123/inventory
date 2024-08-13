using InventoryManagement.Core;
using InventoryManagement.Core.Abstractions;
using InventoryManagement.Infrastructure.Services;
using InventoryManagement.Persistence.Postgres;
using InventoryManagement.Shared.Abstractions.Encryption;
using InventoryManagement.Shared.Infrastructure;
using InventoryManagement.Shared.Infrastructure.Api;
using InventoryManagement.Shared.Infrastructure.Clock;
using InventoryManagement.Shared.Infrastructure.Contexts;
using InventoryManagement.Shared.Infrastructure.Encryption;
using InventoryManagement.Shared.Infrastructure.Files.FileSystems;
using InventoryManagement.Shared.Infrastructure.Initializer;
using InventoryManagement.Shared.Infrastructure.Localization;
using InventoryManagement.Shared.Infrastructure.Serialization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("InventoryManagement.UnitTests")]

namespace InventoryManagement.Infrastructure;

public static class ServiceCollection
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore();
        services.AddSharedInfrastructure();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IFileRepositoryService, FileRepositoryService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IPublisherService, PublisherService>();
        services.AddScoped<IAuthorService, AuthorService>();
        services.AddScoped<IInventoryService, InventoryService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<ITransactionHistoryService, TransactionHistoryService>();
        services.AddSingleton<ISalter, Salter>();

        //use one of these
        //services.AddSqlServerDbContext(configuration, "sqlserver");
        services.AddPostgresDbContext(configuration, "postgres");

        services.AddFileSystemService();
        services.AddJsonSerialization();
        services.AddClock();
        services.AddContext();
        services.AddEncryption();
        services.AddCors();
        services.AddCorsPolicy();
        services.AddLocalizerJson();

        //if use azure blob service
        //make sure app setting "azureBlobService":"" is filled
        //services.AddSingleton<IAzureBlobService, AzureBlobService>();

        services.AddInitializer<CoreInitializer>();
        services.AddApplicationInitializer();
    }
}