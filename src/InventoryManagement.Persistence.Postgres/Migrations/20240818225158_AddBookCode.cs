﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddBookCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Book",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Code",
                table: "Book");
        }
    }
}
