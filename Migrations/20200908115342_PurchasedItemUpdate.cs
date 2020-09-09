using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class PurchasedItemUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "selectedOptionData",
                table: "PurchasedItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "selectedOptionData",
                table: "PurchasedItems");
        }
    }
}
