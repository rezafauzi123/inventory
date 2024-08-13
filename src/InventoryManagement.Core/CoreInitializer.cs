using InventoryManagement.Domain;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Domain.Extensions;
using InventoryManagement.Shared.Abstractions.Clock;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;

namespace InventoryManagement.Core;

public class CoreInitializer : IInitializer
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;
    private readonly IRng _rng;
    private readonly IClock _clock;

    public CoreInitializer(IDbContext dbContext, ISalter salter, IRng rng, IClock clock)
    {
        _dbContext = dbContext;
        _salter = salter;
        _rng = rng;
        _clock = clock;
    }

    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        await AddSuperAdministratorRoleAsync(cancellationToken);

        await AddSuperAdministratorAsync(cancellationToken);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task AddSuperAdministratorRoleAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Set<Role>().AnyAsync(e => e.RoleId == Guid.Empty,
                cancellationToken: cancellationToken))
            return;

        var role = new Role
        {
            IsDefault = true,
            RoleId = RoleExtensions.SuperAdministratorId,
            Name = RoleExtensions.SuperAdministratorName,
            Code = RoleExtensions.Slug(Guid.Empty, RoleExtensions.SuperAdministratorName),
            Description = "Default role to the application"
        };

        await _dbContext.InsertAsync(role, cancellationToken);
    }

    private async Task AddSuperAdministratorAsync(CancellationToken cancellationToken)
    {
        if (await _dbContext.Set<User>().AnyAsync(e => e.UserId == DefaultUser.SuperAdministratorId,
                cancellationToken: cancellationToken))
            return;

        var user = DefaultUser.SuperAdministrator(_rng, _salter, _clock);

        await _dbContext.InsertAsync(user, cancellationToken);
    }
}