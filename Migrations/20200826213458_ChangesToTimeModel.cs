using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NonProfit.Migrations
{
    public partial class ChangesToTimeModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "TimeOnDate",
                table: "Times",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeOnDate",
                table: "Times");
        }
    }
}
