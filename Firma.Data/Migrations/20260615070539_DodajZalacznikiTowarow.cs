using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Firma.Data.Migrations
{
    /// <inheritdoc />
    public partial class DodajZalacznikiTowarow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ZalacznikTowaru",
                columns: table => new
                {
                    IdZalacznikaTowaru = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazwaPliku = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NazwaOryginalna = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Sciezka = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    TypPliku = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Rozmiar = table.Column<long>(type: "bigint", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DataDodania = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CzyAktywny = table.Column<bool>(type: "bit", nullable: false),
                    IdTowaru = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZalacznikTowaru", x => x.IdZalacznikaTowaru);
                    table.ForeignKey(
                        name: "FK_ZalacznikTowaru_Towar_IdTowaru",
                        column: x => x.IdTowaru,
                        principalTable: "Towar",
                        principalColumn: "IdTowaru",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ZalacznikTowaru_IdTowaru",
                table: "ZalacznikTowaru",
                column: "IdTowaru");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ZalacznikTowaru");
        }
    }
}
