using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class RemoveVirtual : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ServiceTypeId",
                table: "ServiceRequests",
                column: "ServiceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceRequests_ServiceTypes_ServiceTypeId",
                table: "ServiceRequests",
                column: "ServiceTypeId",
                principalTable: "ServiceTypes",
                principalColumn: "ServiceTypeId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ServiceRequests_ServiceTypes_ServiceTypeId",
                table: "ServiceRequests");

            migrationBuilder.DropIndex(
                name: "IX_ServiceRequests_ServiceTypeId",
                table: "ServiceRequests");
        }
    }
}
