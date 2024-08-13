using InventoryManagement.Shared.Abstractions.Entities;

namespace InventoryManagement.Domain.Entities;

public sealed class Category : BaseEntity, IEntity
{
    public Category() 
    { 
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}