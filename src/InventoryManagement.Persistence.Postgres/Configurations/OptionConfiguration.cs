using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Persistence.Postgres.Configurations;

public class OptionConfiguration : BaseEntityConfiguration<Option>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Option> builder)
    {
        builder.HasKey(e => e.Key);
        builder.Property(e => e.Key).HasMaxLength(256)
            .ValueGeneratedNever();
        builder.Property(e => e.Value).HasMaxLength(512);
    }
}