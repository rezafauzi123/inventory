using InventoryManagement.Core.Abstractions;
using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.Shared.Abstractions.Encryption;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace InventoryManagement.Infrastructure.Services;

public class BookService : IBookService
{
    private readonly IDbContext _dbContext;
    private readonly ISalter _salter;

    public BookService(IDbContext dbContext, ISalter salter)
    {
        _dbContext = dbContext;
        _salter = salter;
    }

    public IQueryable<Book> GetBaseQuery()
        => _dbContext.Set<Book>()
            .AsQueryable();

    public Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(e => e.Id == id)
            .FirstOrDefaultAsync(cancellationToken);

    public async Task<Book?> CreateAsync(Book entity, CancellationToken cancellationToken = default)
    {
        await _dbContext.InsertAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var Book = await GetByIdAsync(id, cancellationToken);
        if (Book is null)
            throw new Exception("Data not found");

        _dbContext.AttachEntity(Book);

        Book.SetToDeleted();

        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public Task<Book?> GetByExpressionAsync(
        Expression<Func<Book, bool>> predicate,
        Expression<Func<Book, Book>> projection,
        CancellationToken cancellationToken = default)
        => GetBaseQuery()
            .Where(predicate)
            .Select(projection)
            .FirstOrDefaultAsync(cancellationToken);

    public Task<Book?> GetByTitleAsync(string title, CancellationToken cancellationToken = default)
    {
        var s = title.ToUpper();

        return GetBaseQuery()
            .Where(e => e.Title.ToUpper() == s)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> IsBookExistAsync(string title, CancellationToken cancellationToken = default)
    {
        title = title.ToUpper();

        return GetBaseQuery().Where(e => e.Title.ToUpper() == title)
            .AnyAsync(cancellationToken);
    }
}