using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Persistence.Postgres.Configurations;

public class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Category> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name)
            .HasMaxLength(100);       
    }
}