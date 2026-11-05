using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Shopping.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWishlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "shopping");

            migrationBuilder.CreateTable(
                name: "WishlistItems",
                schema: "shopping",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WishlistItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WishlistItems_UserId_ProductId",
                schema: "shopping",
                table: "WishlistItems",
                columns: new[] { "UserId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WishlistItems",
                schema: "shopping");
        }
    }
}
