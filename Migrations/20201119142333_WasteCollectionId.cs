using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class WasteCollectionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WasteCollections",
                table: "WasteCollections");

            migrationBuilder.DropColumn(
                name: "WasteCollectionId",
                table: "WasteCollections");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "WasteCollections",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteCollections",
                table: "WasteCollections",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WasteCollections",
                table: "WasteCollections");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WasteCollections");

            migrationBuilder.AddColumn<string>(
                name: "WasteCollectionId",
                table: "WasteCollections",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteCollections",
                table: "WasteCollections",
                column: "WasteCollectionId");
        }
    }
}
