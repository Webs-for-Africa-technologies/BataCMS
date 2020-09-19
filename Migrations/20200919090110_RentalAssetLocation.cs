using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class RentalAssetLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InStock",
                table: "RentalAssets");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "RentalAssets",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "RentalAssets",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "RentalAssets");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "RentalAssets");

            migrationBuilder.AddColumn<bool>(
                name: "InStock",
                table: "RentalAssets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
