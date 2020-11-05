using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class ServiceRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatchedServices_Services_ServiceRequestId",
                table: "DispatchedServices");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    ServiceRequestId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantName = table.Column<string>(nullable: true),
                    ServiceTypeId = table.Column<int>(nullable: false),
                    ApplicantId = table.Column<string>(nullable: false),
                    ImageUrl = table.Column<string>(nullable: true),
                    ApplicationDate = table.Column<DateTime>(nullable: false),
                    Location = table.Column<string>(nullable: true),
                    RejectMessage = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.ServiceRequestId);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ApplicationUserId",
                table: "ServiceRequests",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchedServices_ServiceRequests_ServiceRequestId",
                table: "DispatchedServices",
                column: "ServiceRequestId",
                principalTable: "ServiceRequests",
                principalColumn: "ServiceRequestId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DispatchedServices_ServiceRequests_ServiceRequestId",
                table: "DispatchedServices");

            migrationBuilder.DropTable(
                name: "ServiceRequests");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    ODServiceId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicantId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicantName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RejectMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ServiceTypeId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.ODServiceId);
                    table.ForeignKey(
                        name: "FK_Services_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_ApplicationUserId",
                table: "Services",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DispatchedServices_Services_ServiceRequestId",
                table: "DispatchedServices",
                column: "ServiceRequestId",
                principalTable: "Services",
                principalColumn: "ODServiceId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
