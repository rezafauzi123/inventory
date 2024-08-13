using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Persistence.Postgres.Configurations;

public class PublisherConfiguration : BaseEntityConfiguration<Publisher>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Publisher> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        builder.Property(e => e.Name)
            .HasMaxLength(100);
        builder.Property(e => e.Phone)
            .HasMaxLength(20);
        builder.Property(e => e.Email)
            .HasMaxLength(50);
        builder.Property(e => e.Address)
            .HasMaxLength(512);
    }
}