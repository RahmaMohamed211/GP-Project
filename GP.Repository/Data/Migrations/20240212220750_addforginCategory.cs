using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GP.Repository.Data.Migrations
{
    public partial class addforginCategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "shipments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_shipments_CategoryId",
                table: "shipments",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_Categories_CategoryId",
                table: "shipments",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_shipments_Categories_CategoryId",
                table: "shipments");

            migrationBuilder.DropIndex(
                name: "IX_shipments_CategoryId",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "shipments");
        }
    }
}
