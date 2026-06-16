using Firma.Data.Data;
using Firma.Data.Data.CMS;
using Firma.Data.Data.Sklep;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Firma.Intranet.Services.Intranet
{
    public static class DaneDemoSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<FirmaContext>();

            await DodajUstawieniaPortalu(context);
            await DodajStronyIAktualnosci(context);
            await DodajPromocje(context);

            var rodzaje = await DodajRodzaje(context);
            var producenci = await DodajProducentow(context);

            await context.SaveChangesAsync();

            await DodajTowary(context, rodzaje, producenci);

            await context.SaveChangesAsync();

            await DodajStanyMagazynowe(context);
            await DodajKlientowIZamowienia(context);

            await context.SaveChangesAsync();
        }

        private static async Task DodajUstawieniaPortalu(FirmaContext context)
        {
            await DodajUstawienie(context, "NazwaPortalu", "3D Print Store", "Nazwa widoczna w portalu publicznym");
            await DodajUstawienie(context, "StopkaTekst", "Sklep z drukarkami 3D, filamentami i akcesoriami.", "Tekst w stopce");
            await DodajUstawienie(context, "StopkaAdres", "ul. Technologiczna 12, 35-001 Rzeszów", "Adres sklepu");
            await DodajUstawienie(context, "StopkaEmail", "kontakt@3dprintstore.pl", "Adres e-mail w stopce");
            await DodajUstawienie(context, "StopkaTelefon", "+48 600 700 800", "Telefon w stopce");
            await DodajUstawienie(context, "StopkaFacebook", "https://facebook.com", "Link do Facebooka");
            await DodajUstawienie(context, "KolorTlaPortalu", "#eef2f6", "Kolor tła portalu");
            await DodajUstawienie(context, "KolorNawigacji", "#ffffff", "Kolor nawigacji");
            await DodajUstawienie(context, "KolorStopki", "#f8f9fa", "Kolor stopki");
            await DodajUstawienie(context, "KolorPrzyciskow", "#0d6efd", "Kolor przycisków");
            await DodajUstawienie(context, "KolorAkcentu", "#258cfb", "Kolor akcentu");
        }

        private static async Task DodajUstawienie(
            FirmaContext context,
            string klucz,
            string wartosc,
            string opis)
        {
            var istnieje = await context.UstawieniePortalu
                .AnyAsync(u => u.Klucz == klucz);

            if (istnieje)
            {
                return;
            }

            context.UstawieniePortalu.Add(new UstawieniePortalu
            {
                Klucz = klucz,
                Wartosc = wartosc,
                Opis = opis,
                CzyAktywny = true
            });
        }

        private static async Task DodajStronyIAktualnosci(FirmaContext context)
        {
            if (!await context.Strona.AnyAsync(s => s.LinkTytul == "Start"))
            {
                var stronyDoPrzesuniecia = await context.Strona
                    .Where(s => s.Pozycja >= 1 && s.Pozycja < 20)
                    .ToListAsync();

                foreach (var strona in stronyDoPrzesuniecia)
                {
                    strona.Pozycja++;
                }

                context.Strona.Add(new Strona
                {
                    LinkTytul = "Start",
                    Tytul = "Druk 3D dla domu, szkoły i firmy",
                    Tresc = "Wybierz drukarkę 3D, filament lub akcesoria i złóż zamówienie online. Portal pokazuje dostępność produktów, pozwala dodać towary do koszyka oraz sprawdzić status zamówienia po numerze i adresie e-mail.",
                    Pozycja = 1,
                    CzyAktywny = true
                });
            }

            await DodajStrone(context, "O sklepie", "O sklepie", "Specjalizujemy się w sprzedaży drukarek 3D, filamentów, części zamiennych i akcesoriów. Oferta jest przygotowana tak, aby klient szybko znalazł produkt, sprawdził dostępność i złożył zamówienie bez kontaktu z administracją.", 2);
            await DodajStrone(context, "Dostawa", "Dostawa i odbiór", "Zamówienia są obsługiwane przez panel Intranet. Po złożeniu zamówienia klient otrzymuje numer, którym może sprawdzić status realizacji w portalu.", 3);
            await DodajStrone(context, "Serwis", "Serwis drukarek 3D", "W ofercie znajdują się również części i akcesoria serwisowe. Produkty mają stany magazynowe, dzięki czemu klient widzi, czy dany element jest dostępny.", 4);

            await DodajAktualnosc(context, "Nowości", "Nowe drukarki 3D w ofercie", "Do oferty dodano drukarki 3D do zastosowań domowych, edukacyjnych i firmowych. Sprawdź dostępność produktów w sklepie.", 1);
            await DodajAktualnosc(context, "Filamenty", "Filamenty PLA i PETG dostępne od ręki", "Najpopularniejsze materiały do druku 3D są dostępne w magazynie. Wybierz kolor i typ materiału dopasowany do projektu.", 2);
            await DodajAktualnosc(context, "Serwis", "Części zamienne i akcesoria serwisowe", "Dysze, płyty robocze i podstawowe zestawy serwisowe możesz dodać do koszyka razem z drukarką.", 3);
        }

        private static async Task DodajStrone(
            FirmaContext context,
            string linkTytul,
            string tytul,
            string tresc,
            int pozycja)
        {
            if (await context.Strona.AnyAsync(s => s.LinkTytul == linkTytul))
            {
                return;
            }

            context.Strona.Add(new Strona
            {
                LinkTytul = linkTytul,
                Tytul = tytul,
                Tresc = tresc,
                Pozycja = pozycja,
                CzyAktywny = true
            });
        }

        private static async Task DodajAktualnosc(
            FirmaContext context,
            string linkTytul,
            string tytul,
            string tresc,
            int pozycja)
        {
            if (await context.Aktualnosc.AnyAsync(a => a.LinkTytul == linkTytul))
            {
                return;
            }

            context.Aktualnosc.Add(new Aktualnosc
            {
                LinkTytul = linkTytul,
                Tytul = tytul,
                Tresc = tresc,
                Pozycja = pozycja,
                CzyAktywny = true
            });
        }

        private static async Task DodajPromocje(FirmaContext context)
        {
            if (!await context.Promocja.AnyAsync(p => p.Tytul == "Zestaw startowy z filamentem"))
            {
                context.Promocja.Add(new Promocja
                {
                    Tytul = "Zestaw startowy z filamentem",
                    Opis = "Kup drukarkę 3D razem z filamentem i akcesoriami startowymi w promocyjnej cenie.",
                    RabatProcentowy = 10,
                    DataOd = DateTime.Today.AddDays(-7),
                    DataDo = DateTime.Today.AddDays(21),
                    CzyAktywny = true
                });
            }

            if (!await context.Promocja.AnyAsync(p => p.Tytul == "Akcesoria serwisowe taniej"))
            {
                context.Promocja.Add(new Promocja
                {
                    Tytul = "Akcesoria serwisowe taniej",
                    Opis = "Dysze, płyty robocze i części eksploatacyjne w niższej cenie dla zamówień z portalu.",
                    RabatProcentowy = 15,
                    DataOd = DateTime.Today.AddDays(-3),
                    DataDo = DateTime.Today.AddDays(14),
                    CzyAktywny = true
                });
            }
        }

        private static async Task<Dictionary<string, Rodzaj>> DodajRodzaje(FirmaContext context)
        {
            var wynik = new Dictionary<string, Rodzaj>();

            wynik["Drukarki 3D"] = await PobierzAlboDodajRodzaj(
                context,
                "Drukarki 3D",
                "Drukarki do domu, szkoły i firmy.");

            wynik["Filamenty"] = await PobierzAlboDodajRodzaj(
                context,
                "Filamenty",
                "Materiały do druku 3D.");

            wynik["Akcesoria"] = await PobierzAlboDodajRodzaj(
                context,
                "Akcesoria",
                "Akcesoria i dodatki do drukarek.");

            wynik["Części"] = await PobierzAlboDodajRodzaj(
                context,
                "Części",
                "Części zamienne i eksploatacyjne.");

            wynik["Serwis"] = await PobierzAlboDodajRodzaj(
                context,
                "Serwis",
                "Zestawy i elementy serwisowe.");

            return wynik;
        }

        private static async Task<Rodzaj> PobierzAlboDodajRodzaj(
            FirmaContext context,
            string nazwa,
            string opis)
        {
            var rodzaj = await context.Rodzaj
                .FirstOrDefaultAsync(r => r.Nazwa == nazwa);

            if (rodzaj != null)
            {
                return rodzaj;
            }

            rodzaj = new Rodzaj
            {
                Nazwa = nazwa,
                Opis = opis,
                CzyAktywny = true
            };

            context.Rodzaj.Add(rodzaj);

            return rodzaj;
        }

        private static async Task<Dictionary<string, Producent>> DodajProducentow(FirmaContext context)
        {
            var wynik = new Dictionary<string, Producent>();

            wynik["Prusa Research"] = await PobierzAlboDodajProducent(
                context,
                "Prusa Research",
                "Czechy",
                "https://www.prusa3d.com",
                "Producent drukarek 3D i rozwiązań dla wymagających użytkowników.");

            wynik["Bambu Lab"] = await PobierzAlboDodajProducent(
                context,
                "Bambu Lab",
                "Chiny",
                "https://bambulab.com",
                "Producent szybkich drukarek 3D dla użytkowników domowych i firm.");

            wynik["Creality"] = await PobierzAlboDodajProducent(
                context,
                "Creality",
                "Chiny",
                "https://www.creality.com",
                "Popularne drukarki 3D i akcesoria w przystępnych cenach.");

            wynik["Fiberlogy"] = await PobierzAlboDodajProducent(
                context,
                "Fiberlogy",
                "Polska",
                "https://fiberlogy.com",
                "Polski producent filamentów do druku 3D.");

            wynik["Noctuo"] = await PobierzAlboDodajProducent(
                context,
                "Noctuo",
                "Polska",
                "https://example.com",
                "Akcesoria i elementy serwisowe dla drukarek 3D.");

            return wynik;
        }

        private static async Task<Producent> PobierzAlboDodajProducent(
            FirmaContext context,
            string nazwa,
            string kraj,
            string stronaWWW,
            string opis)
        {
            var producent = await context.Producent
                .FirstOrDefaultAsync(p => p.Nazwa == nazwa);

            if (producent != null)
            {
                return producent;
            }

            producent = new Producent
            {
                Nazwa = nazwa,
                Kraj = kraj,
                StronaWWW = stronaWWW,
                Opis = opis,
                CzyAktywny = true
            };

            context.Producent.Add(producent);

            return producent;
        }

        private static async Task DodajTowary(
            FirmaContext context,
            Dictionary<string, Rodzaj> rodzaje,
            Dictionary<string, Producent> producenci)
        {
            await DodajTowar(
                context,
                "PR3D-MK4",
                "Prusa MK4S",
                4199.00m,
                "Uniwersalna drukarka 3D do domu, edukacji i prototypowania. Dobry wybór dla osób, które chcą stabilnego sprzętu i wysokiej jakości wydruków.",
                Foto("Prusa MK4S"),
                rodzaje["Drukarki 3D"],
                producenci["Prusa Research"]);

            await DodajTowar(
                context,
                "BBL-P1S",
                "Bambu Lab P1S",
                3799.00m,
                "Szybka drukarka 3D z obudową, przeznaczona do sprawnego drukowania modeli technicznych i użytkowych.",
                Foto("Bambu Lab P1S"),
                rodzaje["Drukarki 3D"],
                producenci["Bambu Lab"]);

            await DodajTowar(
                context,
                "CRE-K1C",
                "Creality K1C",
                2699.00m,
                "Drukarka 3D do szybkiego druku i codziennej pracy z popularnymi materiałami.",
                Foto("Creality K1C"),
                rodzaje["Drukarki 3D"],
                producenci["Creality"]);

            await DodajTowar(
                context,
                "FIL-PLA-MAT",
                "Filament PLA Matt 1kg",
                89.90m,
                "Matowy filament PLA do wydruków dekoracyjnych, makiet i modeli użytkowych.",
                Foto("PLA Matt"),
                rodzaje["Filamenty"],
                producenci["Fiberlogy"]);

            await DodajTowar(
                context,
                "FIL-PETG-CF",
                "Filament PETG CF 0.75kg",
                159.90m,
                "Wzmocniony filament PETG z dodatkiem włókna węglowego do elementów technicznych.",
                Foto("PETG CF"),
                rodzaje["Filamenty"],
                producenci["Fiberlogy"]);

            await DodajTowar(
                context,
                "DYSZA-04",
                "Dysza stalowa 0.4 mm",
                34.90m,
                "Dysza do drukarek 3D, odpowiednia do codziennego drukowania i materiałów technicznych.",
                Foto("Dysza 0.4"),
                rodzaje["Części"],
                producenci["Noctuo"]);

            await DodajTowar(
                context,
                "PLYTA-PEI",
                "Płyta robocza PEI",
                119.00m,
                "Elastyczna płyta robocza PEI poprawiająca przyczepność pierwszej warstwy wydruku.",
                Foto("Płyta PEI"),
                rodzaje["Akcesoria"],
                producenci["Noctuo"]);

            await DodajTowar(
                context,
                "ZEST-SERWIS",
                "Zestaw serwisowy drukarki 3D",
                149.00m,
                "Podstawowy zestaw narzędzi, dysz i elementów eksploatacyjnych do serwisowania drukarki 3D.",
                Foto("Zestaw serwisowy"),
                rodzaje["Serwis"],
                producenci["Noctuo"]);
        }

        private static async Task DodajTowar(
            FirmaContext context,
            string kod,
            string nazwa,
            decimal cena,
            string opis,
            string fotoUrl,
            Rodzaj rodzaj,
            Producent producent)
        {
            if (await context.Towar.AnyAsync(t => t.Kod == kod))
            {
                return;
            }

            context.Towar.Add(new Towar
            {
                Kod = kod,
                Nazwa = nazwa,
                Cena = cena,
                Opis = opis,
                FotoUrl = fotoUrl,
                IdRodzaju = rodzaj.IdRodzaju,
                IdProducenta = producent.IdProducenta,
                CzyAktywny = true
            });
        }

        private static async Task DodajStanyMagazynowe(FirmaContext context)
        {
            await DodajStan(context, "PR3D-MK4", 8, 2, "A1-01");
            await DodajStan(context, "BBL-P1S", 5, 2, "A1-02");
            await DodajStan(context, "CRE-K1C", 11, 3, "A1-03");
            await DodajStan(context, "FIL-PLA-MAT", 64, 10, "B2-01");
            await DodajStan(context, "FIL-PETG-CF", 28, 8, "B2-02");
            await DodajStan(context, "DYSZA-04", 120, 20, "C3-01");
            await DodajStan(context, "PLYTA-PEI", 24, 5, "C3-02");
            await DodajStan(context, "ZEST-SERWIS", 16, 4, "C3-03");
        }

        private static async Task DodajStan(
            FirmaContext context,
            string kodTowaru,
            int ilosc,
            int minimum,
            string lokalizacja)
        {
            var towar = await context.Towar
                .FirstOrDefaultAsync(t => t.Kod == kodTowaru);

            if (towar == null)
            {
                return;
            }

            if (await context.StanMagazynowy.AnyAsync(s => s.IdTowaru == towar.IdTowaru))
            {
                return;
            }

            context.StanMagazynowy.Add(new StanMagazynowy
            {
                IdTowaru = towar.IdTowaru,
                IloscSztuk = ilosc,
                MinimalnaIlosc = minimum,
                Lokalizacja = lokalizacja,
                CzyAktywny = true
            });
        }

        private static async Task DodajKlientowIZamowienia(FirmaContext context)
        {
            var klient = await context.Klient
                .FirstOrDefaultAsync(k => k.Email == "jan.kowalski@example.com");

            if (klient == null)
            {
                klient = new Klient
                {
                    Imie = "Jan",
                    Nazwisko = "Kowalski",
                    Email = "jan.kowalski@example.com",
                    Telefon = "600700800"
                };

                context.Klient.Add(klient);
                await context.SaveChangesAsync();
            }

            if (await context.Zamowienie.AnyAsync(z => z.NumerZamowienia == "WWW-DEMO-001"))
            {
                return;
            }

            var drukarka = await context.Towar
                .FirstOrDefaultAsync(t => t.Kod == "PR3D-MK4");

            var filament = await context.Towar
                .FirstOrDefaultAsync(t => t.Kod == "FIL-PLA-MAT");

            if (drukarka == null || filament == null)
            {
                return;
            }

            var zamowienie = new Zamowienie
            {
                NumerZamowienia = "WWW-DEMO-001",
                DataZamowienia = DateTime.Today.AddDays(-1),
                Status = "w trakcie",
                Ulica = "Kwiatowa",
                NumerDomu = "14",
                NumerLokalu = "3",
                KodPocztowy = "35-001",
                Miasto = "Rzeszów",
                IdKlienta = klient.IdKlienta,
                WartoscRazem = drukarka.Cena + filament.Cena
            };

            zamowienie.PozycjaZamowienia.Add(new PozycjaZamowienia
            {
                IdTowaru = drukarka.IdTowaru,
                Ilosc = 1,
                CenaJednostkowa = drukarka.Cena
            });

            zamowienie.PozycjaZamowienia.Add(new PozycjaZamowienia
            {
                IdTowaru = filament.IdTowaru,
                Ilosc = 1,
                CenaJednostkowa = filament.Cena
            });

            context.Zamowienie.Add(zamowienie);
        }

        private static string Foto(string tekst)
        {
            return $"https://placehold.co/900x700/png?text={Uri.EscapeDataString(tekst)}";
        }
    }
}