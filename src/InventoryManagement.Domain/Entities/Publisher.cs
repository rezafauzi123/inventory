using InventoryManagement.Shared.Abstractions.Entities;

namespace InventoryManagement.Domain.Entities;

public sealed class Publisher : BaseEntity, IEntity
{
    public Publisher() 
    { 
        Id = Guid.NewGuid();
    }
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Address { get; set; }
    public ICollection<Book> Books { get; set; } = new HashSet<Book>();
}