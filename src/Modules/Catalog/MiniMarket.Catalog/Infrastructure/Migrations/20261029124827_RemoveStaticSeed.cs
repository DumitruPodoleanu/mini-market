using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MiniMarket.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStaticSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("a0000000-0000-0000-0000-000000000001") });

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000003"), new Guid("a0000000-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000001"), new Guid("a0000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000004"), new Guid("a0000000-0000-0000-0000-000000000003") });

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "ProductCategories",
                keyColumns: new[] { "CategoryId", "ProductId" },
                keyValues: new object[] { new Guid("c0000000-0000-0000-0000-000000000002"), new Guid("a0000000-0000-0000-0000-000000000004") });

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("c0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Products",
                keyColumn: "Id",
                keyValue: new Guid("a0000000-0000-0000-0000-000000000004"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                schema: "catalog",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("b0000000-0000-0000-0000-000000000003"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
        }
    }
}
