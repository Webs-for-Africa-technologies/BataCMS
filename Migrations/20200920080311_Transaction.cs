using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class Transaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Purchases_PurchaseId",
                table: "PaymentMethods");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchasedItems_Purchases_PurchaseId",
                table: "PurchasedItems");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_PurchasedItems_PurchaseId",
                table: "PurchasedItems");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_PurchaseId",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "PurchasedItems");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "PaymentMethods");

            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "PurchasedItems",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    TransactionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionTotal = table.Column<decimal>(nullable: false),
                    VendorUserId = table.Column<int>(nullable: false),
                    ServerName = table.Column<string>(maxLength: 50, nullable: false),
                    TransactionDate = table.Column<DateTime>(nullable: false),
                    TransactionNotes = table.Column<string>(nullable: true),
                    TransactionType = table.Column<string>(nullable: false),
                    isDelivered = table.Column<bool>(nullable: false),
                    RentalAssetId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_Transactions_RentalAssets_RentalAssetId",
                        column: x => x.RentalAssetId,
                        principalTable: "RentalAssets",
                        principalColumn: "RentalAssetId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RentalAssetId",
                table: "Transactions",
                column: "RentalAssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "PurchasedItems");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "PurchasedItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "PaymentMethods",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PurchaseNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PurchasesTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ServerName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    isDelivered = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchasedItems_PurchaseId",
                table: "PurchasedItems",
                column: "PurchaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_PurchaseId",
                table: "PaymentMethods",
                column: "PurchaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Purchases_PurchaseId",
                table: "PaymentMethods",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchasedItems_Purchases_PurchaseId",
                table: "PurchasedItems",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
