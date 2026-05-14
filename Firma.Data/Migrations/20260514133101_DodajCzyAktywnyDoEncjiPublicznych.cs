using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodajCzyAktywnyDoEncjiPublicznych : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CzyAktywny",
                table: "UstawieniePortalu",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CzyAktywny",
                table: "Towar",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CzyAktywny",
                table: "Strona",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CzyAktywny",
                table: "StanMagazynowy",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CzyAktywny",
                table: "Rodzaj",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CzyAktywny",
                table: "Promocja",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CzyAktywny",
                table: "Producent",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CzyAktywny",
                table: "Aktualnosc",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CzyAktywny",
                table: "UstawieniePortalu");

            migrationBuilder.DropColumn(
                name: "CzyAktywny",
                table: "Towar");

            migrationBuilder.DropColumn(
                name: "CzyAktywny",
                table: "Strona");

            migrationBuilder.DropColumn(
                name: "CzyAktywny",
                table: "StanMagazynowy");

            migrationBuilder.DropColumn(
                name: "CzyAktywny",
                table: "Rodzaj");

            migrationBuilder.DropColumn(
                name: "CzyAktywny",
                table: "Promocja");

            migrationBuilder.DropColumn(
                name: "CzyAktywny",
                table: "Producent");

            migrationBuilder.DropColumn(
                name: "CzyAktywny",
                table: "Aktualnosc");
        }
    }
}
