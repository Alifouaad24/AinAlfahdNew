using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AinAlfahd.Migrations
{
    /// <inheritdoc />
    public partial class AddDataToTblConfigAndCil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Db_env",
                table: "tbl_config",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "tbl_config",
                columns: new[] { "id", "Db_env", "missing_fav" },
                values: new object[] { 5, "Dev", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "tbl_config",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Db_env",
                table: "tbl_config");
        }
    }
}
