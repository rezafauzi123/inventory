using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class RoleService : IRoleService
{
    private readonly IDbContext _dbContext;

    public RoleService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<Role> GetBaseQuery()
        => _dbContext.Set<Role>()
            .Where(e => e.IsDefault == false)
            .AsQueryable();

    public Task<Role?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.RoleId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Role?> CreateAsync(Role entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var role = await GetByExpressionAsync(
            e => e.RoleId == id,
            e => new Role
            {
                RoleId = e.RoleId
            }, cancellationToken);

        if (role is null)
            throw new Exception("Data role not found");

        _dbContext.AttachEntity(role);

        role.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Role?> GetByExpressionAsync(
        Expression<Func<Role, bool>> predicate,
        Expression<Func<Role, Role>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Role?> GetByCodeAsync(string code, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Code == code)
            .FirstOrDefaultAsync(cancellationToken);
}