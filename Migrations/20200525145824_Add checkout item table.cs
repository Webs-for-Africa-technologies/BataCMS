using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class Addcheckoutitemtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckoutItems",
                columns: table => new
                {
                    CheckoutItemId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    unitItemId = table.Column<int>(nullable: true),
                    Amount = table.Column<int>(nullable: false),
                    CheckoutId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckoutItems", x => x.CheckoutItemId);
                    table.ForeignKey(
                        name: "FK_CheckoutItems_UnitItems_unitItemId",
                        column: x => x.unitItemId,
                        principalTable: "UnitItems",
                        principalColumn: "unitItemId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckoutItems_unitItemId",
                table: "CheckoutItems",
                column: "unitItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckoutItems");
        }
    }
}
