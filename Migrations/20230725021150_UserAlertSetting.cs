using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceChecker.Migrations
{
    /// <inheritdoc />
    public partial class UserAlertSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserAlertSettings",
                columns: table => new
                {
                    AlertID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserID = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    LowBalanceAlertEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LowBalanceThreshold = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    HighBalanceAlertEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HighBalanceThreshold = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    OverspendingAlertEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    DueDateReminderAlertEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IncomeDepositedAlertEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAlertSettings", x => x.AlertID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserAlertSettings");
        }
    }
}
