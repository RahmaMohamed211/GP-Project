using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Repository.Data.Migrations
{
    public partial class updateShipemt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Shipment_ShipmentId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_City_FromCityID",
                table: "Shipment");

            migrationBuilder.DropForeignKey(
                name: "FK_Shipment_City_ToCityId",
                table: "Shipment");

            migrationBuilder.DropIndex(
                name: "IX_Products_ShipmentId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment");

            migrationBuilder.DropColumn(
                name: "ShipmentId",
                table: "Products");

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

            migrationBuilder.CreateTable(
                name: "ProductShipment",
                columns: table => new
                {
                    ProductsId = table.Column<int>(type: "int", nullable: false),
                    shipmentsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductShipment", x => new { x.ProductsId, x.shipmentsId });
                    table.ForeignKey(
                        name: "FK_ProductShipment_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductShipment_shipments_shipmentsId",
                        column: x => x.shipmentsId,
                        principalTable: "shipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductShipment_shipmentsId",
                table: "ProductShipment",
                column: "shipmentsId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shipments_City_FromCityID",
                table: "shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_shipments_City_ToCityId",
                table: "shipments");

            migrationBuilder.DropTable(
                name: "ProductShipment");

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

            migrationBuilder.AddColumn<int>(
                name: "ShipmentId",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Shipment",
                table: "Shipment",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ShipmentId",
                table: "Products",
                column: "ShipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Shipment_ShipmentId",
                table: "Products",
                column: "ShipmentId",
                principalTable: "Shipment",
                principalColumn: "Id");

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
    }
}
