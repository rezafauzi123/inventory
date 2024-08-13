using InventoryManagement.Domain.Entities;
using InventoryManagement.Shared.Abstractions.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InventoryManagement.Persistence.Postgres.Configurations;

public class BookConfiguration : BaseEntityConfiguration<Book>
{
    protected override void EntityConfiguration(EntityTypeBuilder<Book> builder)
    {        
        builder.HasKey(b => b.Id);
     
        builder.Property(b => b.Title)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(b => b.Isbn)
            .IsRequired()
            .HasMaxLength(13);

        builder.Property(b => b.Language)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(b => b.Dimensions)
            .IsRequired();

        builder.Property(b => b.Price)
            .IsRequired()
            .HasDefaultValue(0);

        builder.Property(b => b.PublishedDate)
            .IsRequired();

        builder.Property(b => b.Year)
            .IsRequired();
        
        builder.HasOne(b => b.Author)
            .WithMany(a => a.Books)
            .HasForeignKey(b => b.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Category)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(b => b.PublisherId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(b => b.Inventory)
            .WithOne(i => i.Book)
            .HasForeignKey<Inventory>(i => i.BookId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasIndex(b => b.Isbn).IsUnique();
        builder.HasIndex(b => b.Title);
    }
}