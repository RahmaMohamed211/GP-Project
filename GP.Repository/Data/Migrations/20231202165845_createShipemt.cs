using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Repository.Data.Migrations
{
    public partial class createShipemt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShipmentId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Shipment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reward = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Dateofcreation = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    FromCityID = table.Column<int>(type: "int", nullable: false),
                    ToCityId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfRecieving = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Shipment_City_FromCityID",
                        column: x => x.FromCityID,
                        principalTable: "City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Shipment_City_ToCityId",
                        column: x => x.ToCityId,
                        principalTable: "City",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShipmentId",
                table: "Products",
                column: "ShipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_FromCityID",
                table: "Shipment",
                column: "FromCityID");

            migrationBuilder.CreateIndex(
                name: "IX_Shipment_ToCityId",
                table: "Shipment",
                column: "ToCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shipment_ShipmentId",
                table: "Products",
                column: "ShipmentId",
                principalTable: "Shipment",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shipment_ShipmentId",
                table: "Products");

            migrationBuilder.DropTable(
                name: "Shipment");

            migrationBuilder.DropIndex(
                name: "IX_Products_ShipmentId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "Products");
        }
    }
}
