using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class VendorApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantId",
                table: "VendorApplications",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "VendorApplications",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VendorApplications_ApplicationUserId",
                table: "VendorApplications",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_VendorApplications_AspNetUsers_ApplicationUserId",
                table: "VendorApplications",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VendorApplications_AspNetUsers_ApplicationUserId",
                table: "VendorApplications");

            migrationBuilder.DropIndex(
                name: "IX_VendorApplications_ApplicationUserId",
                table: "VendorApplications");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "VendorApplications");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "VendorApplications");
        }
    }
}
