using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class FileRepositoryService : IFileRepositoryService
{
    private readonly IDbContext _dbContext;

    public FileRepositoryService(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IQueryable<FileRepository> GetBaseQuery() =>
        _dbContext.Set<FileRepository>()
            .AsQueryable();

    public Task<FileRepository?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.FileRepositoryId == id)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<List<FileRepository>> GetAllAsync(CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .ToListAsync(cancellationToken);

    public async Task<FileRepository?> CreateAsync(FileRepository entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _dbContext.Set<FileRepository>()
            .Select(e => new FileRepository
            {
                FileRepositoryId = e.FileRepositoryId
            })
            .Where(e => e.FileRepositoryId == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (entity is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(entity);

        entity.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<FileRepository?> GetByExpressionAsync(
        Expression<Func<FileRepository, bool>> predicate,
        Expression<Func<FileRepository, FileRepository>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<long> TotalUsedStorageAsync(CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.IsFileDeleted == false)
            .SumAsync(e => e.Size, cancellationToken);

    public Task<FileRepository?> GetByUniqueFileNameAsync(string fileName,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.UniqueFileName == fileName)
            .FirstOrDefaultAsync(cancellationToken);
}