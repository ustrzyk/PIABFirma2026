using Firma.Data.Data.CMS;
using Firma.Data.Data.Sklep;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Firma.Data.Data
{
    public class FirmaContext : DbContext
    {
        public FirmaContext(DbContextOptions<FirmaContext> options) : base(options)
        {
        }

        public DbSet<Aktualnosc> Aktualnosc { get; set; } = default!;
        public DbSet<Promocja> Promocja { get; set; } = default!;
        public DbSet<Strona> Strona { get; set; } = default!;
        public DbSet<UstawieniePortalu> UstawieniePortalu { get; set; } = default!;

        public DbSet<Producent> Producent { get; set; } = default!;
        public DbSet<Rodzaj> Rodzaj { get; set; } = default!;
        public DbSet<Towar> Towar { get; set; } = default!;
        public DbSet<StanMagazynowy> StanMagazynowy { get; set; } = default!;
        public DbSet<Klient> Klient { get; set; } = default!;
        public DbSet<Zamowienie> Zamowienie { get; set; } = default!;
        public DbSet<PozycjaZamowienia> PozycjaZamowienia { get; set; } = default!;
        public DbSet<ZalacznikTowaru> ZalacznikTowaru { get; set; } = default!;
    }
}