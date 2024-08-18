using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InventoryManagement.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class AddBookCodeInventory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookCode",
                table: "Inventory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BookTitle",
                table: "Inventory",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookCode",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "BookTitle",
                table: "Inventory");
        }
    }
}
