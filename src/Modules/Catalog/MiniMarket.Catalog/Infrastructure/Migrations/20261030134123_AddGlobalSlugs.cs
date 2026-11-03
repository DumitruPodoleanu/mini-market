using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniMarket.Catalog.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddGlobalSlugs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Slugs",
                schema: "catalog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    EntityType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Slugs", x => x.Id);
                });

            migrationBuilder.AddColumn<Guid>(
                name: "SlugId",
                schema: "catalog",
                table: "Brands",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SlugId",
                schema: "catalog",
                table: "Categories",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SlugId",
                schema: "catalog",
                table: "Products",
                type: "uuid",
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE catalog."Brands" SET "SlugId" = gen_random_uuid();
                INSERT INTO catalog."Slugs" ("Id", "Value", "EntityType")
                SELECT "SlugId", "Slug", 'Brand' FROM catalog."Brands";

                UPDATE catalog."Categories" SET "SlugId" = gen_random_uuid();
                INSERT INTO catalog."Slugs" ("Id", "Value", "EntityType")
                SELECT "SlugId", "Slug", 'Category' FROM catalog."Categories";

                UPDATE catalog."Products" SET "SlugId" = gen_random_uuid();
                INSERT INTO catalog."Slugs" ("Id", "Value", "EntityType")
                SELECT "SlugId",
                       btrim(regexp_replace(regexp_replace(lower("Name"), '[''./]', '', 'g'), '[^a-z0-9]+', '-', 'g'), '-'),
                       'Product'
                FROM catalog."Products";
                """);

            migrationBuilder.AlterColumn<Guid>(
                name: "SlugId",
                schema: "catalog",
                table: "Brands",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SlugId",
                schema: "catalog",
                table: "Categories",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SlugId",
                schema: "catalog",
                table: "Products",
                type: "uuid",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.DropIndex(
                name: "IX_Brands_Slug",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Categories_Slug",
                schema: "catalog",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "Slug",
                schema: "catalog",
                table: "Categories");

            migrationBuilder.CreateIndex(
                name: "IX_Slugs_Value",
                schema: "catalog",
                table: "Slugs",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Brands_SlugId",
                schema: "catalog",
                table: "Brands",
                column: "SlugId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SlugId",
                schema: "catalog",
                table: "Categories",
                column: "SlugId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_SlugId",
                schema: "catalog",
                table: "Products",
                column: "SlugId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Brands_Slugs_SlugId",
                schema: "catalog",
                table: "Brands",
                column: "SlugId",
                principalSchema: "catalog",
                principalTable: "Slugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Slugs_SlugId",
                schema: "catalog",
                table: "Categories",
                column: "SlugId",
                principalSchema: "catalog",
                principalTable: "Slugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Slugs_SlugId",
                schema: "catalog",
                table: "Products",
                column: "SlugId",
                principalSchema: "catalog",
                principalTable: "Slugs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Brands_Slugs_SlugId",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Slugs_SlugId",
                schema: "catalog",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Slugs_SlugId",
                schema: "catalog",
                table: "Products");

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "catalog",
                table: "Brands",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Slug",
                schema: "catalog",
                table: "Categories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.Sql("""
                UPDATE catalog."Brands" b SET "Slug" = s."Value" FROM catalog."Slugs" s WHERE s."Id" = b."SlugId";
                UPDATE catalog."Categories" c SET "Slug" = s."Value" FROM catalog."Slugs" s WHERE s."Id" = c."SlugId";
                """);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "catalog",
                table: "Brands",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Slug",
                schema: "catalog",
                table: "Categories",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.DropIndex(
                name: "IX_Brands_SlugId",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SlugId",
                schema: "catalog",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Products_SlugId",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SlugId",
                schema: "catalog",
                table: "Brands");

            migrationBuilder.DropColumn(
                name: "SlugId",
                schema: "catalog",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SlugId",
                schema: "catalog",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Slugs",
                schema: "catalog");

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
        }
    }
}
