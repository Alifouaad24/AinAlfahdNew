using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AinAlfahd.Migrations
{
    /// <inheritdoc />
    public partial class EditRecieptTb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Recirpts");

            migrationBuilder.CreateTable(
                name: "Reciepts",
                columns: table => new
                {
                    RecieptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    RecieptDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DisCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SellingPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SellingCurrency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SellingDisCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FinanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinanced = table.Column<bool>(type: "bit", nullable: false),
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reciepts", x => x.RecieptId);
                    table.ForeignKey(
                        name: "FK_Reciepts_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reciepts_CustomerId",
                table: "Reciepts",
                column: "CustomerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reciepts");

            migrationBuilder.CreateTable(
                name: "Recirpts",
                columns: table => new
                {
                    RecirptId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisCount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InsertDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    Weight = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recirpts", x => x.RecirptId);
                    table.ForeignKey(
                        name: "FK_Recirpts_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recirpts_CustomerId",
                table: "Recirpts",
                column: "CustomerId");
        }
    }
}
