using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class AddpurchasePaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_PaymentMethods_PaymentMethodId",
                table: "Purchases");

            migrationBuilder.DropIndex(
                name: "IX_Purchases_PaymentMethodId",
                table: "Purchases");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Purchases");

            migrationBuilder.AddColumn<int>(
                name: "PurchaseId",
                table: "PaymentMethods",
                nullable: true);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Purchases_PurchaseId",
                table: "PaymentMethods");

            migrationBuilder.DropIndex(
                name: "IX_PaymentMethods_PurchaseId",
                table: "PaymentMethods");

            migrationBuilder.DropColumn(
                name: "PurchaseId",
                table: "PaymentMethods");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Purchases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_PaymentMethodId",
                table: "Purchases",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_PaymentMethods_PaymentMethodId",
                table: "Purchases",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
