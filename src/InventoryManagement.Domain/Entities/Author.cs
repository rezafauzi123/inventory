using InventoryManagement.Shared.Abstractions.Entities;

namespace InventoryManagement.Domain.Entities;

public sealed class Author : BaseEntity, IEntity
{
    public Author() 
    { 
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Biography { get; set; }
    public string? Image { get; set; }
    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}