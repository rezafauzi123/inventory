﻿using InventoryManagement.Shared.Abstractions.Entities;

namespace InventoryManagement.Domain.Entities;

public sealed class Inventory : BaseEntity, IEntity
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid BookId { get; set; }
    public string BookCode { get; set; } = null!;
    public string BookTitle { get; set; } = null!;
    public int Stock { get; set; }
    public Book? Book { get; set; }
}