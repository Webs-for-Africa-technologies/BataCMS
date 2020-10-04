using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class TranscationLeaseId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Leases_LeaseId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "LeaseId",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Leases_LeaseId",
                table: "Transactions",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "LeaseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Leases_LeaseId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "LeaseId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Leases_LeaseId",
                table: "Transactions",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "LeaseId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
