using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class Invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    InvoiceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RentalAssetId = table.Column<int>(nullable: false),
                    ApplicationId = table.Column<string>(nullable: true),
                    LeaseFrom = table.Column<DateTime>(nullable: false),
                    LeaseTo = table.Column<DateTime>(nullable: false),
                    AmountPaid = table.Column<decimal>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.InvoiceId);
                    table.ForeignKey(
                        name: "FK_Invoices_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Invoices_RentalAssets_RentalAssetId",
                        column: x => x.RentalAssetId,
                        principalTable: "RentalAssets",
                        principalColumn: "RentalAssetId",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_ApplicationUserId",
                table: "Invoices",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_RentalAssetId",
                table: "Invoices",
                column: "RentalAssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Invoices");
        }
    }
}
