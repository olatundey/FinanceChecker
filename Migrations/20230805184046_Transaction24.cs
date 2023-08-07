using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceChecker.Migrations
{
    /// <inheritdoc />
    public partial class Transaction24 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBalanceUpdate",
                table: "Transactions",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBalanceUpdate",
                table: "Transactions");
        }
    }
}
