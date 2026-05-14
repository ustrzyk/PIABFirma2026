using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class M01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Aktualnosc",
                columns: table => new
                {
                    IdAktualnosci = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkTytul = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Tytul = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Tresc = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Pozycja = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Aktualnosc", x => x.IdAktualnosci);
                });

            migrationBuilder.CreateTable(
                name: "Klient",
                columns: table => new
                {
                    IdKlienta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Imie = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nazwisko = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Telefon = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klient", x => x.IdKlienta);
                });

            migrationBuilder.CreateTable(
                name: "Producent",
                columns: table => new
                {
                    IdProducenta = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Kraj = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StronaWWW = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Producent", x => x.IdProducenta);
                });

            migrationBuilder.CreateTable(
                name: "Promocja",
                columns: table => new
                {
                    IdPromocji = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tytul = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    RabatProcentowy = table.Column<int>(type: "int", nullable: false),
                    DataOd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataDo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promocja", x => x.IdPromocji);
                });

            migrationBuilder.CreateTable(
                name: "Rodzaj",
                columns: table => new
                {
                    IdRodzaju = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nazwa = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rodzaj", x => x.IdRodzaju);
                });

            migrationBuilder.CreateTable(
                name: "Strona",
                columns: table => new
                {
                    IdStrony = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LinkTytul = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Tytul = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Tresc = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Pozycja = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Strona", x => x.IdStrony);
                });

            migrationBuilder.CreateTable(
                name: "UstawieniePortalu",
                columns: table => new
                {
                    IdUstawieniaPortalu = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Klucz = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Wartosc = table.Column<string>(type: "nvarchar(MAX)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UstawieniePortalu", x => x.IdUstawieniaPortalu);
                });

            migrationBuilder.CreateTable(
                name: "Zamowienie",
                columns: table => new
                {
                    IdZamowienia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NumerZamowienia = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DataZamowienia = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    WartoscRazem = table.Column<decimal>(type: "money", nullable: false),
                    Ulica = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    NumerDomu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumerLokalu = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    KodPocztowy = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Miasto = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IdKlienta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zamowienie", x => x.IdZamowienia);
                    table.ForeignKey(
                        name: "FK_Zamowienie_Klient_IdKlienta",
                        column: x => x.IdKlienta,
                        principalTable: "Klient",
                        principalColumn: "IdKlienta",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Towar",
                columns: table => new
                {
                    IdTowaru = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Kod = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Nazwa = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: false),
                    Cena = table.Column<decimal>(type: "money", nullable: false),
                    FotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IdRodzaju = table.Column<int>(type: "int", nullable: false),
                    IdProducenta = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Towar", x => x.IdTowaru);
                    table.ForeignKey(
                        name: "FK_Towar_Producent_IdProducenta",
                        column: x => x.IdProducenta,
                        principalTable: "Producent",
                        principalColumn: "IdProducenta",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Towar_Rodzaj_IdRodzaju",
                        column: x => x.IdRodzaju,
                        principalTable: "Rodzaj",
                        principalColumn: "IdRodzaju",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PozycjaZamowienia",
                columns: table => new
                {
                    IdPozycjiZamowienia = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ilosc = table.Column<int>(type: "int", nullable: false),
                    CenaJednostkowa = table.Column<decimal>(type: "money", nullable: false),
                    IdZamowienia = table.Column<int>(type: "int", nullable: false),
                    IdTowaru = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PozycjaZamowienia", x => x.IdPozycjiZamowienia);
                    table.ForeignKey(
                        name: "FK_PozycjaZamowienia_Towar_IdTowaru",
                        column: x => x.IdTowaru,
                        principalTable: "Towar",
                        principalColumn: "IdTowaru",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PozycjaZamowienia_Zamowienie_IdZamowienia",
                        column: x => x.IdZamowienia,
                        principalTable: "Zamowienie",
                        principalColumn: "IdZamowienia",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StanMagazynowy",
                columns: table => new
                {
                    IdStanuMagazynowego = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IloscSztuk = table.Column<int>(type: "int", nullable: false),
                    MinimalnaIlosc = table.Column<int>(type: "int", nullable: false),
                    Lokalizacja = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    IdTowaru = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StanMagazynowy", x => x.IdStanuMagazynowego);
                    table.ForeignKey(
                        name: "FK_StanMagazynowy_Towar_IdTowaru",
                        column: x => x.IdTowaru,
                        principalTable: "Towar",
                        principalColumn: "IdTowaru",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PozycjaZamowienia_IdTowaru",
                table: "PozycjaZamowienia",
                column: "IdTowaru");

            migrationBuilder.CreateIndex(
                name: "IX_PozycjaZamowienia_IdZamowienia",
                table: "PozycjaZamowienia",
                column: "IdZamowienia");

            migrationBuilder.CreateIndex(
                name: "IX_StanMagazynowy_IdTowaru",
                table: "StanMagazynowy",
                column: "IdTowaru",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Towar_IdProducenta",
                table: "Towar",
                column: "IdProducenta");

            migrationBuilder.CreateIndex(
                name: "IX_Towar_IdRodzaju",
                table: "Towar",
                column: "IdRodzaju");

            migrationBuilder.CreateIndex(
                name: "IX_Zamowienie_IdKlienta",
                table: "Zamowienie",
                column: "IdKlienta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Aktualnosc");

            migrationBuilder.DropTable(
                name: "PozycjaZamowienia");

            migrationBuilder.DropTable(
                name: "Promocja");

            migrationBuilder.DropTable(
                name: "StanMagazynowy");

            migrationBuilder.DropTable(
                name: "Strona");

            migrationBuilder.DropTable(
                name: "UstawieniePortalu");

            migrationBuilder.DropTable(
                name: "Zamowienie");

            migrationBuilder.DropTable(
                name: "Towar");

            migrationBuilder.DropTable(
                name: "Klient");

            migrationBuilder.DropTable(
                name: "Producent");

            migrationBuilder.DropTable(
                name: "Rodzaj");
        }
    }
}
