using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AinAlfahd.Migrations
{
    /// <inheritdoc />
    public partial class AddUniqueConstraintOnCustMob : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.CreateIndex(
                name: "IX_customers_cust_mob",
                table: "customers",
                column: "cust_mob",
                unique: true,
                filter: "[cust_mob] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_CustMob",
                table: "Customers");
        }
    }
}
