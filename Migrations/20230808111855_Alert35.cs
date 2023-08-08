using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceChecker.Migrations
{
    /// <inheritdoc />
    public partial class Alert35 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SavingsGoalProgressAlertEnabled",
                table: "UserAlertSettings",
                newName: "TargetAmountReachedAlertEnabled");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TargetAmountReachedAlertEnabled",
                table: "UserAlertSettings",
                newName: "SavingsGoalProgressAlertEnabled");
        }
    }
}
