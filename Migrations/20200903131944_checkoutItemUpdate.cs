using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class checkoutItemUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "selectedOptions",
                table: "CheckoutItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "selectedOptions",
                table: "CheckoutItems");
        }
    }
}
