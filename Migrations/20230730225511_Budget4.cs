using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceChecker.Migrations
{
    /// <inheritdoc />
    public partial class Budget4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FAQs",
                columns: new[] { "Id", "Answer", "Question" },
                values: new object[] { 4, "Our app allows you to track expenses easily. First, categorise your transactions on the \"Transactions\" section,  then go to the \"Expense Tracking\" section, and view detailed reports of your spending.", "How can I track my expenses?" });
        }
    }
}
