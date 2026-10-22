using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniMarket.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog");

            migrationBuilder.CreateTable(
                name: "Brands",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Slug = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Sku = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Published = table.Column<bool>(type: "boolean", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    BrandId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalSchema: "catalog",
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                schema: "catalog",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "catalog",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "Brands",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("b0000000-0000-0000-0000-000000000001"), "Now Foods", "now-foods" },
                    { new Guid("b0000000-0000-0000-0000-000000000002"), "Solgar", "solgar" },
                    { new Guid("b0000000-0000-0000-0000-000000000003"), "Nature's Way", "natures-way" }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "Categories",
                columns: new[] { "Id", "Name", "Slug" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), "Suplimente", "suplimente" },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), "Plante adaptogene", "plante-adaptogene" },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), "Vitamine", "vitamine" },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), "Minerale", "minerale" }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "Products",
                columns: new[] { "Id", "BrandId", "Name", "Price", "Published", "Sku", "StockQuantity" },
                values: new object[,]
                {
                    { new Guid("a0000000-0000-0000-0000-000000000001"), new Guid("b0000000-0000-0000-0000-000000000001"), "Ashwagandha 60 caps", 189.90m, true, "HERB-001", 25 },
                    { new Guid("a0000000-0000-0000-0000-000000000002"), new Guid("b0000000-0000-0000-0000-000000000002"), "Vitamina D3 2000 UI 120 caps", 149.50m, true, "VIT-001", 40 },
                    { new Guid("a0000000-0000-0000-0000-000000000003"), new Guid("b0000000-0000-0000-0000-000000000003"), "Magneziu bisglicinat 90 tab", 219.00m, true, "MIN-001", 12 },
                    { new Guid("a0000000-0000-0000-0000-000000000004"), new Guid("b0000000-0000-0000-0000-000000000001"), "Rhodiola 60 caps", 249.00m, false, "HERB-002", 0 }
                });

            migrationBuilder.InsertData(
                schema: "catalog",
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("a0000000-0000-0000-0000-000000000001") },
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000002") },
                    { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("a0000000-0000-0000-0000-000000000002") },
                    { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000003") },
                    { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("a0000000-0000-0000-0000-000000000003") },
                    { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("a0000000-0000-0000-0000-000000000004") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Brands_Slug",
                schema: "catalog",
                table: "Brands",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Slug",
                schema: "catalog",
                table: "Categories",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryId",
                schema: "catalog",
                table: "ProductCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                schema: "catalog",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                schema: "catalog",
                table: "Products",
                column: "Sku",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductCategories",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "catalog");

            migrationBuilder.DropTable(
                name: "Brands",
                schema: "catalog");
        }
    }
}
