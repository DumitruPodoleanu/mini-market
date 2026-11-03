using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVendors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vendors",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SlugId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vendors_Slugs_SlugId",
                        column: x => x.SlugId,
                        principalSchema: "catalog",
                        principalTable: "Slugs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vendors_SlugId",
                schema: "catalog",
                table: "Vendors",
                column: "SlugId",
                unique: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VendorId",
                schema: "catalog",
                table: "Products",
                type: "uuid",
                nullable: true);

            var vendorSlugId = Guid.CreateVersion7();
            var vendorId = Guid.CreateVersion7();

            migrationBuilder.Sql($"""
                INSERT INTO catalog."Slugs" ("Id", "Value", "EntityType")
                SELECT '{vendorSlugId}', 'mini-market', 'Vendor'
                WHERE EXISTS (SELECT 1 FROM catalog."Products");

                INSERT INTO catalog."Vendors" ("Id", "Name", "SlugId")
                SELECT '{vendorId}', 'Mini Market', '{vendorSlugId}'
                WHERE EXISTS (SELECT 1 FROM catalog."Products");

                UPDATE catalog."Products"
                SET "VendorId" = '{vendorId}';
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "VendorId",
                schema: "catalog",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.DropIndex(
                name: "IX_Products_Sku",
                schema: "catalog",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_VendorId_Sku",
                schema: "catalog",
                table: "Products",
                columns: new[] { "VendorId", "Sku" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Vendors_VendorId",
                schema: "catalog",
                table: "Products",
                column: "VendorId",
                principalSchema: "catalog",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Vendors_VendorId",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_VendorId_Sku",
                schema: "catalog",
                table: "Products");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Sku",
                schema: "catalog",
                table: "Products",
                column: "Sku",
                unique: true);

            migrationBuilder.DropColumn(
                name: "VendorId",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Vendors",
                schema: "catalog");

            migrationBuilder.Sql("""
                DELETE FROM catalog."Slugs" WHERE "EntityType" = 'Vendor';
                """);
        }
    }
}
