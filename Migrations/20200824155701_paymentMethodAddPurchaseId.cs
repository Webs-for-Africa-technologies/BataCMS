using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class paymentMethodAddPurchaseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Purchases_PurchaseId",
                table: "PaymentMethods");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseId",
                table: "PaymentMethods",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Purchases_PurchaseId",
                table: "PaymentMethods",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PaymentMethods_Purchases_PurchaseId",
                table: "PaymentMethods");

            migrationBuilder.AlterColumn<int>(
                name: "PurchaseId",
                table: "PaymentMethods",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PaymentMethods_Purchases_PurchaseId",
                table: "PaymentMethods",
                column: "PurchaseId",
                principalTable: "Purchases",
                principalColumn: "PurchaseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
