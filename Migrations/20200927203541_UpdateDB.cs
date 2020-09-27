using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class UpdateDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckoutItems");

            migrationBuilder.DropTable(
                name: "Currencies");

            migrationBuilder.DropTable(
                name: "PurchasedItems");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckoutItems",
                columns: table => new
                {
                    CheckoutItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    CheckoutId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RentalAssetId = table.Column<int>(type: "int", nullable: true),
                    selectedOptions = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutItems", x => x.CheckoutItemId);
                    table.ForeignKey(
                        name: "FK_CheckoutItems_RentalAssets_RentalAssetId",
                        column: x => x.RentalAssetId,
                        principalTable: "RentalAssets",
                        principalColumn: "RentalAssetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Currencies",
                columns: table => new
                {
                    CurrencyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrencyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    isCurrent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currencies", x => x.CurrencyId);
                });

            migrationBuilder.CreateTable(
                name: "PurchasedItems",
                columns: table => new
                {
                    PurchasedItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RentalAssetId = table.Column<int>(type: "int", nullable: true),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    selectedOptionData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    unitItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchasedItems", x => x.PurchasedItemId);
                    table.ForeignKey(
                        name: "FK_PurchasedItems_RentalAssets_RentalAssetId",
                        column: x => x.RentalAssetId,
                        principalTable: "RentalAssets",
                        principalColumn: "RentalAssetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutItems_RentalAssetId",
                table: "CheckoutItems",
                column: "RentalAssetId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedItems_RentalAssetId",
                table: "PurchasedItems",
                column: "RentalAssetId");
        }
    }
}
