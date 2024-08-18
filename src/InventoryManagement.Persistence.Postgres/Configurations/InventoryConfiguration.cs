using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace InventoryManagement.Persistence.Postgres.Configurations;

public class InventoryConfiguration : BaseEntityConfiguration<Inventory>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Inventory> builder)
    {        
        builder.HasKey(i => i.Id);

        builder.Property(i => i.BookCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.BookTitle)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(i => i.Stock)
            .IsRequired()
            .HasDefaultValue(0);
        
        builder.HasOne(i => i.Book)
            .WithOne(b => b.Inventory)
            .HasForeignKey<Inventory>(i => i.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(i => i.BookId).IsUnique();
    }
}