using InventoryManagement.Shared.Abstractions.Entities;

namespace InventoryManagement.Domain.Entities;

public sealed class TransactionHistory : BaseEntity, IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid InventoryId { get; set; }
    public DateTime TransactionDate { get; set; }
    public int TransactionType { get; set; }
    public int Qty { get; set; }
    public Inventory? Inventory { get; set; }
}