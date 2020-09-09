using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class unitItemUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OptionFormData",
                table: "UnitItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OptionFormData",
                table: "UnitItems");
        }
    }
}
