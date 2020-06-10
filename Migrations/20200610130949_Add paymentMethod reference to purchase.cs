using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class AddpaymentMethodreferencetopurchase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PaymentMethodId",
                table: "Purchases",
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
