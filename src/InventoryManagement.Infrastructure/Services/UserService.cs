using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class UserService : IUserService
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;

    public UserService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
        _salter = salter;
    }

    public IQueryable<User> GetBaseQuery()
        => _dbContext.Set<User>()
            .Include(e => e.UserRoles)
            .ThenInclude(e => e.Role!.RoleScopes)
            .AsQueryable();

    public Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.UserId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<User?> CreateAsync(User entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await GetByIdAsync(id, cancellationToken);
        if (user is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(user);

        user.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<User?> GetByExpressionAsync(
        Expression<Func<User, bool>> predicate,
        Expression<Func<User, User>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        var s = username.ToUpper();

        return GetBaseQuery()
            .Where(e => e.NormalizedUsername == s)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsUserExistAsync(string username, CancellationToken cancellationToken = default)
    {
        username = username.ToUpper();

        return GetBaseQuery().Where(e => e.NormalizedUsername == username)
            .AnyAsync(cancellationToken);
    }

    public bool VerifyPassword(string currentPassword, string salt, string password)
        => currentPassword == _salter.Hash(salt, password);
}