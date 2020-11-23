using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class WasteCollections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_WasteCollectionTable",
                table: "WasteCollectionTable");

            migrationBuilder.RenameTable(
                name: "WasteCollectionTable",
                newName: "WasteCollections");

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

            migrationBuilder.RenameTable(
                name: "WasteCollections",
                newName: "WasteCollectionTable");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WasteCollectionTable",
                table: "WasteCollectionTable",
                column: "WasteCollectionId");
        }
    }
}
