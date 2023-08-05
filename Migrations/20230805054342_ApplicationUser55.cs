using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceChecker.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationUser55 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SecurityAnswer",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "SecurityQuestion",
                table: "AspNetUsers",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Answer",
                value: "To sign up for our app, simply click on the \"Register\" button on the homepage, fill in the required information, and create a password to enable you login.");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SecurityAnswer",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SecurityQuestion",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "FAQs",
                keyColumn: "Id",
                keyValue: 1,
                column: "Answer",
                value: "To sign up for our app, simply click on the \"Register\" button on the homepage, fill in the required information, and create a password. You'll receive a verification email to activate your account.");
        }
    }
}
