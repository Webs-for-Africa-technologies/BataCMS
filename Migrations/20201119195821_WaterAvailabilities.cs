using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class WaterAvailabilities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WaterAvailabilities",
                columns: table => new
                {
                    WaterAvailabilityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceArea = table.Column<string>(nullable: true),
                    DateModified = table.Column<DateTime>(nullable: false),
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
                    table.PrimaryKey("PK_WaterAvailabilities", x => x.WaterAvailabilityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WaterAvailabilities");
        }
    }
}
