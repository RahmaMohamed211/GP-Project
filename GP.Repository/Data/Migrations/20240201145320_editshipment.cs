using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Repository.Data.Migrations
{
    public partial class editshipment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shipments_Country_FromCountryID",
                table: "shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_shipments_Country_ToCountryId",
                table: "shipments");

            migrationBuilder.DropIndex(
                name: "IX_shipments_FromCountryID",
                table: "shipments");

            migrationBuilder.DropIndex(
                name: "IX_shipments_ToCountryId",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "FromCountryID",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "ToCountryId",
                table: "shipments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FromCountryID",
                table: "shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ToCountryId",
                table: "shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_shipments_FromCountryID",
                table: "shipments",
                column: "FromCountryID");

            migrationBuilder.CreateIndex(
                name: "IX_shipments_ToCountryId",
                table: "shipments",
                column: "ToCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_Country_FromCountryID",
                table: "shipments",
                column: "FromCountryID",
                principalTable: "Country",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_Country_ToCountryId",
                table: "shipments",
                column: "ToCountryId",
                principalTable: "Country",
                principalColumn: "Id");
        }
    }
}
