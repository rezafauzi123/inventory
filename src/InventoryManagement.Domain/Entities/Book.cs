using InventoryManagement.Shared.Abstractions.Entities;

namespace InventoryManagement.Domain.Entities;

public sealed class Book : BaseEntity, IEntity
{
    public Book()
    {
        Id = Guid.NewGuid();
    }

    public Guid Id { get; set; }
    public Guid AuthorId { get; set; }
    public Guid CategoryId { get; set; }
    public Guid PublisherId { get; set; }
    public string Code { get; set; }
    public string Title { get; set; } = null!;
    public int Year { get; set; }
    public int Pages { get; set; }
    public string? Description { get; set; }
    public DateTime PublishedDate { get; set; }
    public string Isbn { get; set; } = null!;
    public string Dimensions { get; set; } = null!;
    public int Weight { get; set; }
    public int Price { get; set; }
    public string? Cover { get; set; }
    public string Language { get; set; } = null!;
    public Author? Author { get; set; }
    public Category? Category { get; set; }
    public Publisher? Publisher { get; set; }
    public Inventory? Inventory { get; set; }
}