using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Repository.Data.Migrations
{
    public partial class createTrip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductShipment_shipments_shipmentsId",
                table: "ProductShipment");

            migrationBuilder.DropForeignKey(
                name: "FK_shipments_City_FromCityID",
                table: "shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_shipments_City_ToCityId",
                table: "shipments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_shipments",
                table: "shipments");

            migrationBuilder.RenameTable(
                name: "shipments",
                newName: "Shipment");

            migrationBuilder.RenameIndex(
                name: "IX_shipments_ToCityId",
                table: "Shipment",
                newName: "IX_Shipment_ToCityId");

            migrationBuilder.RenameIndex(
                name: "IX_shipments_FromCityID",
                table: "Shipment",
                newName: "IX_Shipment_FromCityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Trip",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromCityID = table.Column<int>(type: "int", nullable: false),
                    ToCityId = table.Column<int>(type: "int", nullable: false),
                    availableKg = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    arrivalTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    DateofCreation = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trip", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trip_City_FromCityID",
                        column: x => x.FromCityID,
                        principalTable: "City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Trip_City_ToCityId",
                        column: x => x.ToCityId,
                        principalTable: "City",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trip_FromCityID",
                table: "Trip",
                column: "FromCityID");

            migrationBuilder.CreateIndex(
                name: "IX_Trip_ToCityId",
                table: "Trip",
                column: "ToCityId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductShipment_Shipment_shipmentsId",
                table: "ProductShipment",
                column: "shipmentsId",
                principalTable: "Shipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_City_FromCityID",
                table: "Shipment",
                column: "FromCityID",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shipment_City_ToCityId",
                table: "Shipment",
                column: "ToCityId",
                principalTable: "City",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductShipment_Shipment_shipmentsId",
                table: "ProductShipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_City_FromCityID",
                table: "Shipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_City_ToCityId",
                table: "Shipment");

            migrationBuilder.DropTable(
                name: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment");

            migrationBuilder.RenameTable(
                name: "Shipment",
                newName: "shipments");

            migrationBuilder.RenameIndex(
                name: "IX_Shipment_ToCityId",
                table: "shipments",
                newName: "IX_shipments_ToCityId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipment_FromCityID",
                table: "shipments",
                newName: "IX_shipments_FromCityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_shipments",
                table: "shipments",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductShipment_shipments_shipmentsId",
                table: "ProductShipment",
                column: "shipmentsId",
                principalTable: "shipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_City_FromCityID",
                table: "shipments",
                column: "FromCityID",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_City_ToCityId",
                table: "shipments",
                column: "ToCityId",
                principalTable: "City",
                principalColumn: "Id");
        }
    }
}
