using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class TransactionLease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_RentalAssets_RentalAssetId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RentalAssetId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RentalAssetId",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "LeaseId",
                table: "Transactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_LeaseId",
                table: "Transactions",
                column: "LeaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Leases_LeaseId",
                table: "Transactions",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "LeaseId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Leases_LeaseId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_LeaseId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "LeaseId",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "RentalAssetId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RentalAssetId",
                table: "Transactions",
                column: "RentalAssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_RentalAssets_RentalAssetId",
                table: "Transactions",
                column: "RentalAssetId",
                principalTable: "RentalAssets",
                principalColumn: "RentalAssetId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
