using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class WasteCollectionIdAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WasteCollections",
                table: "WasteCollections");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WasteCollections");

            migrationBuilder.AddColumn<int>(
                name: "WasteCollectionId",
                table: "WasteCollections",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteCollections",
                table: "WasteCollections",
                column: "WasteCollectionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteCollections",
                table: "WasteCollections",
                column: "Id");
        }
    }
}
