using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Repository.Data.Migrations
{
    public partial class edit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_City_FromCityID",
                table: "Trip");

            migrationBuilder.DropForeignKey(
                name: "FK_Trip_City_ToCityId",
                table: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trip",
                table: "Trip");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment");

            migrationBuilder.RenameTable(
                name: "Trip",
                newName: "Trips");

            migrationBuilder.RenameTable(
                name: "Shipment",
                newName: "shipments");

            migrationBuilder.RenameIndex(
                name: "IX_Trip_ToCityId",
                table: "Trips",
                newName: "IX_Trips_ToCityId");

            migrationBuilder.RenameIndex(
                name: "IX_Trip_FromCityID",
                table: "Trips",
                newName: "IX_Trips_FromCityID");

            migrationBuilder.RenameIndex(
                name: "IX_Shipment_ToCityId",
                table: "shipments",
                newName: "IX_shipments_ToCityId");

            migrationBuilder.RenameIndex(
                name: "IX_Shipment_FromCityID",
                table: "shipments",
                newName: "IX_shipments_FromCityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trips",
                table: "Trips",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_City_FromCityID",
                table: "Trips",
                column: "FromCityID",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trips_City_ToCityId",
                table: "Trips",
                column: "ToCityId",
                principalTable: "City",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_City_FromCityID",
                table: "Trips");

            migrationBuilder.DropForeignKey(
                name: "FK_Trips_City_ToCityId",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Trips",
                table: "Trips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_shipments",
                table: "shipments");

            migrationBuilder.RenameTable(
                name: "Trips",
                newName: "Trip");

            migrationBuilder.RenameTable(
                name: "shipments",
                newName: "Shipment");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_ToCityId",
                table: "Trip",
                newName: "IX_Trip_ToCityId");

            migrationBuilder.RenameIndex(
                name: "IX_Trips_FromCityID",
                table: "Trip",
                newName: "IX_Trip_FromCityID");

            migrationBuilder.RenameIndex(
                name: "IX_shipments_ToCityId",
                table: "Shipment",
                newName: "IX_Shipment_ToCityId");

            migrationBuilder.RenameIndex(
                name: "IX_shipments_FromCityID",
                table: "Shipment",
                newName: "IX_Shipment_FromCityID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Trip",
                table: "Trip",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_City_FromCityID",
                table: "Trip",
                column: "FromCityID",
                principalTable: "City",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Trip_City_ToCityId",
                table: "Trip",
                column: "ToCityId",
                principalTable: "City",
                principalColumn: "Id");
        }
    }
}
