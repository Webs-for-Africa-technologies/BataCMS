using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class DispatchedServicesRequestId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatchedServices_Services_ServiceRequestODServiceId",
                table: "DispatchedServices");

            migrationBuilder.DropIndex(
                name: "IX_DispatchedServices_ServiceRequestODServiceId",
                table: "DispatchedServices");

            migrationBuilder.DropColumn(
                name: "ServiceRequestODServiceId",
                table: "DispatchedServices");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceRequestId",
                table: "DispatchedServices",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DispatchedServices_ServiceRequestId",
                table: "DispatchedServices",
                column: "ServiceRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchedServices_Services_ServiceRequestId",
                table: "DispatchedServices",
                column: "ServiceRequestId",
                principalTable: "Services",
                principalColumn: "ODServiceId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatchedServices_Services_ServiceRequestId",
                table: "DispatchedServices");

            migrationBuilder.DropIndex(
                name: "IX_DispatchedServices_ServiceRequestId",
                table: "DispatchedServices");

            migrationBuilder.AlterColumn<string>(
                name: "ServiceRequestId",
                table: "DispatchedServices",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "ServiceRequestODServiceId",
                table: "DispatchedServices",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DispatchedServices_ServiceRequestODServiceId",
                table: "DispatchedServices",
                column: "ServiceRequestODServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchedServices_Services_ServiceRequestODServiceId",
                table: "DispatchedServices",
                column: "ServiceRequestODServiceId",
                principalTable: "Services",
                principalColumn: "ODServiceId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
