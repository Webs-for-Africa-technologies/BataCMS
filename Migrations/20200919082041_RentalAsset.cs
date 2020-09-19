using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class RentalAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutItems_UnitItems_unitItemId",
                table: "CheckoutItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedItems_UnitItems_unitItemId",
                table: "PurchasedItems");

            migrationBuilder.DropTable(
                name: "UnitItems");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedItems_unitItemId",
                table: "PurchasedItems");

            migrationBuilder.DropIndex(
                name: "IX_CheckoutItems_unitItemId",
                table: "CheckoutItems");

            migrationBuilder.DropColumn(
                name: "unitItemId",
                table: "CheckoutItems");

            migrationBuilder.AddColumn<int>(
                name: "RentalAssetId",
                table: "PurchasedItems",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RentalAssetId",
                table: "CheckoutItems",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RentalAssets",
                columns: table => new
                {
                    RentalAssetId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    InStock = table.Column<bool>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OptionFormData = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalAssets", x => x.RentalAssetId);
                    table.ForeignKey(
                        name: "FK_RentalAssets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedItems_RentalAssetId",
                table: "PurchasedItems",
                column: "RentalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutItems_RentalAssetId",
                table: "CheckoutItems",
                column: "RentalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalAssets_CategoryId",
                table: "RentalAssets",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutItems_RentalAssets_RentalAssetId",
                table: "CheckoutItems",
                column: "RentalAssetId",
                principalTable: "RentalAssets",
                principalColumn: "RentalAssetId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedItems_RentalAssets_RentalAssetId",
                table: "PurchasedItems",
                column: "RentalAssetId",
                principalTable: "RentalAssets",
                principalColumn: "RentalAssetId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckoutItems_RentalAssets_RentalAssetId",
                table: "CheckoutItems");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedItems_RentalAssets_RentalAssetId",
                table: "PurchasedItems");

            migrationBuilder.DropTable(
                name: "RentalAssets");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedItems_RentalAssetId",
                table: "PurchasedItems");

            migrationBuilder.DropIndex(
                name: "IX_CheckoutItems_RentalAssetId",
                table: "CheckoutItems");

            migrationBuilder.DropColumn(
                name: "RentalAssetId",
                table: "PurchasedItems");

            migrationBuilder.DropColumn(
                name: "RentalAssetId",
                table: "CheckoutItems");

            migrationBuilder.AddColumn<int>(
                name: "unitItemId",
                table: "CheckoutItems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnitItems",
                columns: table => new
                {
                    unitItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InStock = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OptionFormData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitItems", x => x.unitItemId);
                    table.ForeignKey(
                        name: "FK_UnitItems_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedItems_unitItemId",
                table: "PurchasedItems",
                column: "unitItemId");

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutItems_unitItemId",
                table: "CheckoutItems",
                column: "unitItemId");

            migrationBuilder.CreateIndex(
                name: "IX_UnitItems_CategoryId",
                table: "UnitItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckoutItems_UnitItems_unitItemId",
                table: "CheckoutItems",
                column: "unitItemId",
                principalTable: "UnitItems",
                principalColumn: "unitItemId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedItems_UnitItems_unitItemId",
                table: "PurchasedItems",
                column: "unitItemId",
                principalTable: "UnitItems",
                principalColumn: "unitItemId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
