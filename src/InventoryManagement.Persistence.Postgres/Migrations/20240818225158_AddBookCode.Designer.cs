﻿// <auto-generated />
using System;
using InventoryManagement.Persistence.Postgres;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InventoryManagement.Persistence.Postgres.Migrations
{
    [DbContext(typeof(PostgresDbContext))]
    [Migration("20240818225158_AddBookCode")]
    partial class AddBookCode
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Author", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Biography")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CreatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("CreatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Image")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("LastUpdatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("LastUpdatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("StatusRecord")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Author");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Book", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Cover")
                        .HasColumnType("text");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CreatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("CreatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Dimensions")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<string>("Isbn")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("LastUpdatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("LastUpdatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("Pages")
                        .HasColumnType("integer");

                    b.Property<int>("Price")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("PublisherId")
                        .HasColumnType("uuid");

                    b.Property<int>("StatusRecord")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("Weight")
                        .HasColumnType("integer");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.HasIndex("CategoryId");

                    b.HasIndex("Isbn")
                        .IsUnique();

                    b.HasIndex("PublisherId");

                    b.HasIndex("Title");

                    b.ToTable("Book");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CreatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("CreatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("LastUpdatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("LastUpdatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("StatusRecord")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.FileRepository", b =>
                {
                    b.Property<Guid>("FileRepositoryId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CreatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("CreatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("FileDeletedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("FileStoreAt")
                        .HasColumnType("integer");

                    b.Property<int>("FileType")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsFileDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("LastUpdatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("LastUpdatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Note")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<string>("Source")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("StatusRecord")
                        .HasColumnType("integer");

                    b.Property<string>("UniqueFileName")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("FileRepositoryId");

                    b.HasIndex("UniqueFileName")
                        .IsUnique();

                    b.ToTable("FileRepository");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Inventory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("BookId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CreatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("CreatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("LastUpdatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("LastUpdatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("StatusRecord")
                        .HasColumnType("integer");

                    b.Property<int>("Stock")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.HasIndex("BookId")
                        .IsUnique();

                    b.ToTable("Inventory");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Publisher", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CreatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("CreatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("LastUpdatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("LastUpdatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<int>("StatusRecord")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Publisher");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.TransactionHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("CreatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("CreatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("CreatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid>("InventoryId")
                        .HasMaxLength(100)
                        .HasColumnType("uuid");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("LastUpdatedAtServer")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LastUpdatedBy")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("LastUpdatedByFullName")
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)");

                    b.Property<string>("LastUpdatedByName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<int>("Qty")
                        .HasColumnType("integer");

                    b.Property<int>("StatusRecord")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("TransactionType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("InventoryId");

                    b.ToTable("TransactionHistory");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Book", b =>
                {
                    b.HasOne("InventoryManagement.Domain.Entities.Author", "Author")
                        .WithMany("Books")
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("InventoryManagement.Domain.Entities.Category", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("InventoryManagement.Domain.Entities.Publisher", "Publisher")
                        .WithMany("Books")
                        .HasForeignKey("PublisherId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Category");

                    b.Navigation("Publisher");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Inventory", b =>
                {
                    b.HasOne("InventoryManagement.Domain.Entities.Book", "Book")
                        .WithOne("Inventory")
                        .HasForeignKey("InventoryManagement.Domain.Entities.Inventory", "BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.TransactionHistory", b =>
                {
                    b.HasOne("InventoryManagement.Domain.Entities.Inventory", "Inventory")
                        .WithMany()
                        .HasForeignKey("InventoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Author", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Book", b =>
                {
                    b.Navigation("Inventory");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Category", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("InventoryManagement.Domain.Entities.Publisher", b =>
                {
                    b.Navigation("Books");
                });
#pragma warning restore 612, 618
        }
    }
}
