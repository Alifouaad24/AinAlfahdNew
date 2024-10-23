using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AinAlfahd.Migrations
{
    /// <inheritdoc />
    public partial class AddDisCountToReciptTB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "DisCount",
                table: "Recirpts",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisCount",
                table: "Recirpts");
        }
    }
}
