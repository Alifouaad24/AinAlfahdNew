using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AinAlfahd.Migrations
{
    /// <inheritdoc />
    public partial class addOldCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OldCode",
                table: "items",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_order_details_item_code",
                table: "order_details",
                column: "item_code");

            migrationBuilder.AddForeignKey(
                name: "FK_order_details_items_item_code",
                table: "order_details",
                column: "item_code",
                principalTable: "items",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_order_details_items_item_code",
                table: "order_details");

            migrationBuilder.DropIndex(
                name: "IX_order_details_item_code",
                table: "order_details");

            migrationBuilder.DropColumn(
                name: "OldCode",
                table: "items");
        }
    }
}
