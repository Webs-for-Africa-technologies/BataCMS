using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class ActiveLease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveLeases",
                columns: table => new
                {
                    ActiveLeaseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalAssetId = table.Column<int>(nullable: false),
                    LeaseId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveLeases", x => x.ActiveLeaseId);
                    table.ForeignKey(
                        name: "FK_ActiveLeases_Leases_LeaseId",
                        column: x => x.LeaseId,
                        principalTable: "Leases",
                        principalColumn: "LeaseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ActiveLeases_RentalAssets_RentalAssetId",
                        column: x => x.RentalAssetId,
                        principalTable: "RentalAssets",
                        principalColumn: "RentalAssetId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ActiveLeases_LeaseId",
                table: "ActiveLeases",
                column: "LeaseId");

            migrationBuilder.CreateIndex(
                name: "IX_ActiveLeases_RentalAssetId",
                table: "ActiveLeases",
                column: "RentalAssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveLeases");
        }
    }
}
