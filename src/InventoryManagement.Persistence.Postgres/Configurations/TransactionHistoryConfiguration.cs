using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Persistence.Postgres.Configurations;

public class TransactionHistoryConfiguration : BaseEntityConfiguration<TransactionHistory>
{
    protected override void EntityConfiguration(EntityTypeBuilder<TransactionHistory> builder)
    {
        builder.HasKey(e => new { e.Id });
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.InventoryId).
            HasMaxLength(100);   
    }
}