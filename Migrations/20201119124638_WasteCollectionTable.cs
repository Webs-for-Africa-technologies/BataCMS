using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class WasteCollectionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WasteCollectionTable",
                columns: table => new
                {
                    WasteCollectionId = table.Column<string>(nullable: false),
                    ServiceArea = table.Column<string>(nullable: true),
                    Mon = table.Column<bool>(nullable: false),
                    Tue = table.Column<bool>(nullable: false),
                    Wed = table.Column<bool>(nullable: false),
                    Thur = table.Column<bool>(nullable: false),
                    Fri = table.Column<bool>(nullable: false),
                    Sat = table.Column<bool>(nullable: false),
                    Sun = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WasteCollectionTable", x => x.WasteCollectionId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WasteCollectionTable");
        }
    }
}
