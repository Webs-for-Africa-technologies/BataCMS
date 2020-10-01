﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BataCMS.Migrations
{
    public partial class RentalAssetUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "BookTillDate",
                table: "RentalAssets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookTillDate",
                table: "RentalAssets");
        }
    }
}
