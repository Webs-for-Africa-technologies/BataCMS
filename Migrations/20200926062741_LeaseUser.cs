using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class LeaseUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VendorUserId",
                table: "Leases");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Leases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Leases");

            migrationBuilder.AddColumn<int>(
                name: "VendorUserId",
                table: "Leases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
