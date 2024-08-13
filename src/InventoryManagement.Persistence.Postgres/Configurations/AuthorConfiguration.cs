using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Persistence.Postgres.Configurations;

public class AuthorConfiguration : BaseEntityConfiguration<Author>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Author> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name)
            .HasMaxLength(100);
        builder.Property(e => e.Biography)
            .HasMaxLength(512);
        builder.Property(e => e.Image)
            .HasMaxLength(512);        
    }
}