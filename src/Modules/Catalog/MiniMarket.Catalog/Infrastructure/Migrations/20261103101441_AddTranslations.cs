using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTranslations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryTranslations",
                schema: "catalog",
                columns: table => new
                {
                    Language = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryTranslations", x => new { x.CategoryId, x.Language });
                    table.ForeignKey(
                        name: "FK_CategoryTranslations_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "catalog",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTranslations",
                schema: "catalog",
                columns: table => new
                {
                    Language = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTranslations", x => new { x.ProductId, x.Language });
                    table.ForeignKey(
                        name: "FK_ProductTranslations_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryTranslations",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "ProductTranslations",
                schema: "catalog");
        }
    }
}
