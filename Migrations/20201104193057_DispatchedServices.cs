using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class DispatchedServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DispatchedServices",
                columns: table => new
                {
                    DispatchedServiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    ServiceRequestId = table.Column<string>(nullable: true),
                    DispatchTime = table.Column<DateTime>(nullable: false),
                    ServiceRequestODServiceId = table.Column<int>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DispatchedServices", x => x.DispatchedServiceId);
                    table.ForeignKey(
                        name: "FK_DispatchedServices_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DispatchedServices_Services_ServiceRequestODServiceId",
                        column: x => x.ServiceRequestODServiceId,
                        principalTable: "Services",
                        principalColumn: "ODServiceId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DispatchedServices_ApplicationUserId",
                table: "DispatchedServices",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_DispatchedServices_ServiceRequestODServiceId",
                table: "DispatchedServices",
                column: "ServiceRequestODServiceId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DispatchedServices");
        }
    }
}
