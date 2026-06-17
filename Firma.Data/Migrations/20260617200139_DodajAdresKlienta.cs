using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodajAdresKlienta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KodPocztowy",
                table: "Klient",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Miasto",
                table: "Klient",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumerDomu",
                table: "Klient",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NumerLokalu",
                table: "Klient",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ulica",
                table: "Klient",
                type: "nvarchar(80)",
                maxLength: 80,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KodPocztowy",
                table: "Klient");

            migrationBuilder.DropColumn(
                name: "Miasto",
                table: "Klient");

            migrationBuilder.DropColumn(
                name: "NumerDomu",
                table: "Klient");

            migrationBuilder.DropColumn(
                name: "NumerLokalu",
                table: "Klient");

            migrationBuilder.DropColumn(
                name: "Ulica",
                table: "Klient");
        }
    }
}
