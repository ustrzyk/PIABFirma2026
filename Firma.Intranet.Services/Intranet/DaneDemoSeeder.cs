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

            await DodajLubAktualizujUstawieniaPortalu(context);
            await DodajLubAktualizujStronyIAktualnosci(context);
            await DodajLubAktualizujPromocje(context);

            var rodzaje = await DodajLubAktualizujRodzaje(context);
            var producenci = await DodajLubAktualizujProducentow(context);

            await context.SaveChangesAsync();

            await DodajLubAktualizujTowary(context, rodzaje, producenci);

            await context.SaveChangesAsync();

            await DodajLubAktualizujStanyMagazynowe(context);
            await DodajLubAktualizujKlientowIZamowienia(context);

            await context.SaveChangesAsync();
        }

        private static async Task DodajLubAktualizujUstawieniaPortalu(FirmaContext context)
        {
            await Ustawienie(context, "NazwaPortalu", "3D Print Store", "Nazwa widoczna w portalu publicznym");
            await Ustawienie(context, "StopkaTekst", "Sklep z drukarkami 3D, filamentami, częściami i akcesoriami.", "Tekst w stopce");
            await Ustawienie(context, "StopkaAdres", "ul. Technologiczna 12, 35-001 Rzeszów", "Adres sklepu");
            await Ustawienie(context, "StopkaEmail", "kontakt@3dprintstore.pl", "Adres e-mail w stopce");
            await Ustawienie(context, "StopkaTelefon", "+48 600 700 800", "Telefon w stopce");
            await Ustawienie(context, "StopkaFacebook", "https://facebook.com", "Link do Facebooka");
            await Ustawienie(context, "KolorTlaPortalu", "#eef2f6", "Kolor tła portalu");
            await Ustawienie(context, "KolorNawigacji", "#ffffff", "Kolor nawigacji");
            await Ustawienie(context, "KolorStopki", "#f8f9fa", "Kolor stopki");
            await Ustawienie(context, "KolorPrzyciskow", "#0d6efd", "Kolor przycisków");
            await Ustawienie(context, "KolorAkcentu", "#258cfb", "Kolor akcentu");
        }

        private static async Task Ustawienie(
            FirmaContext context,
            string klucz,
            string wartosc,
            string opis)
        {
            var ustawienie = await context.UstawieniePortalu
                .FirstOrDefaultAsync(u => u.Klucz == klucz);

            if (ustawienie == null)
            {
                context.UstawieniePortalu.Add(new UstawieniePortalu
                {
                    Klucz = klucz,
                    Wartosc = wartosc,
                    Opis = opis,
                    CzyAktywny = true
                });

                return;
            }

            if (string.IsNullOrWhiteSpace(ustawienie.Wartosc))
            {
                ustawienie.Wartosc = wartosc;
            }

            if (string.IsNullOrWhiteSpace(ustawienie.Opis))
            {
                ustawienie.Opis = opis;
            }

            ustawienie.CzyAktywny = true;
        }

        private static async Task DodajLubAktualizujStronyIAktualnosci(FirmaContext context)
        {
            await Strona(
                context,
                "Start",
                "Druk 3D dla domu, szkoły i firmy",
                "Wybierz drukarkę 3D, filament lub akcesoria i złóż zamówienie online. Portal pokazuje dostępność produktów, pozwala dodać towary do koszyka oraz sprawdzić status zamówienia po numerze i adresie e-mail.",
                1);

            await Strona(
                context,
                "O sklepie",
                "O sklepie",
                "Specjalizujemy się w sprzedaży drukarek 3D, filamentów, części zamiennych i akcesoriów. Oferta jest przygotowana tak, aby klient szybko znalazł produkt, sprawdził dostępność i złożył zamówienie.",
                2);

            await Strona(
                context,
                "Dostawa",
                "Dostawa i odbiór",
                "Zamówienia są obsługiwane przez panel Intranet. Po złożeniu zamówienia klient otrzymuje numer, którym może sprawdzić status realizacji w portalu.",
                3);

            await Strona(
                context,
                "Serwis",
                "Serwis drukarek 3D",
                "W ofercie znajdują się również części i akcesoria serwisowe. Produkty mają stany magazynowe, dzięki czemu klient widzi, czy dany element jest dostępny.",
                4);

            await Aktualnosc(
                context,
                "Nowości",
                "Nowe drukarki 3D w ofercie",
                "Do oferty dodano drukarki 3D do zastosowań domowych, edukacyjnych i firmowych. Sprawdź dostępność produktów w sklepie.",
                1);

            await Aktualnosc(
                context,
                "Filamenty",
                "Filamenty PLA i PETG dostępne od ręki",
                "Najpopularniejsze materiały do druku 3D są dostępne w magazynie. Wybierz kolor i typ materiału dopasowany do projektu.",
                2);

            await Aktualnosc(
                context,
                "Serwis",
                "Części zamienne i akcesoria serwisowe",
                "Dysze, płyty robocze i podstawowe zestawy serwisowe możesz dodać do koszyka razem z drukarką.",
                3);
        }

        private static async Task Strona(
            FirmaContext context,
            string linkTytul,
            string tytul,
            string tresc,
            int pozycja)
        {
            var strona = await context.Strona
                .FirstOrDefaultAsync(s => s.LinkTytul == linkTytul);

            if (strona == null)
            {
                context.Strona.Add(new Strona
                {
                    LinkTytul = linkTytul,
                    Tytul = tytul,
                    Tresc = tresc,
                    Pozycja = pozycja,
                    CzyAktywny = true
                });

                return;
            }

            strona.Tytul = tytul;
            strona.Tresc = tresc;
            strona.Pozycja = pozycja;
            strona.CzyAktywny = true;
        }

        private static async Task Aktualnosc(
            FirmaContext context,
            string linkTytul,
            string tytul,
            string tresc,
            int pozycja)
        {
            var aktualnosc = await context.Aktualnosc
                .FirstOrDefaultAsync(a => a.LinkTytul == linkTytul);

            if (aktualnosc == null)
            {
                context.Aktualnosc.Add(new Aktualnosc
                {
                    LinkTytul = linkTytul,
                    Tytul = tytul,
                    Tresc = tresc,
                    Pozycja = pozycja,
                    CzyAktywny = true
                });

                return;
            }

            aktualnosc.Tytul = tytul;
            aktualnosc.Tresc = tresc;
            aktualnosc.Pozycja = pozycja;
            aktualnosc.CzyAktywny = true;
        }

        private static async Task DodajLubAktualizujPromocje(FirmaContext context)
        {
            await Promocja(
                context,
                "Zestaw startowy z filamentem",
                "Kup drukarkę 3D razem z filamentem i akcesoriami startowymi w promocyjnej cenie.",
                10,
                DateTime.Today.AddDays(-7),
                DateTime.Today.AddDays(21));

            await Promocja(
                context,
                "Akcesoria serwisowe taniej",
                "Dysze, płyty robocze i części eksploatacyjne w niższej cenie dla zamówień z portalu.",
                15,
                DateTime.Today.AddDays(-3),
                DateTime.Today.AddDays(14));
        }

        private static async Task Promocja(
            FirmaContext context,
            string tytul,
            string opis,
            int rabatProcentowy,
            DateTime dataOd,
            DateTime dataDo)
        {
            var promocja = await context.Promocja
                .FirstOrDefaultAsync(p => p.Tytul == tytul);

            if (promocja == null)
            {
                context.Promocja.Add(new Promocja
                {
                    Tytul = tytul,
                    Opis = opis,
                    RabatProcentowy = rabatProcentowy,
                    DataOd = dataOd,
                    DataDo = dataDo,
                    CzyAktywny = true
                });

                return;
            }

            promocja.Opis = opis;
            promocja.RabatProcentowy = rabatProcentowy;
            promocja.DataOd = dataOd;
            promocja.DataDo = dataDo;
            promocja.CzyAktywny = true;
        }

        private static async Task<Dictionary<string, Rodzaj>> DodajLubAktualizujRodzaje(FirmaContext context)
        {
            var wynik = new Dictionary<string, Rodzaj>();

            wynik["Drukarki 3D"] = await Rodzaj(
                context,
                "Drukarki 3D",
                "Drukarki do domu, szkoły i firmy.");

            wynik["Filamenty"] = await Rodzaj(
                context,
                "Filamenty",
                "Materiały do druku 3D.");

            wynik["Akcesoria"] = await Rodzaj(
                context,
                "Akcesoria",
                "Akcesoria i dodatki do drukarek.");

            wynik["Części"] = await Rodzaj(
                context,
                "Części",
                "Części zamienne i eksploatacyjne.");

            wynik["Serwis"] = await Rodzaj(
                context,
                "Serwis",
                "Zestawy i elementy serwisowe.");

            return wynik;
        }

        private static async Task<Rodzaj> Rodzaj(
            FirmaContext context,
            string nazwa,
            string opis)
        {
            var rodzaj = await context.Rodzaj
                .FirstOrDefaultAsync(r => r.Nazwa == nazwa);

            if (rodzaj == null)
            {
                rodzaj = new Rodzaj
                {
                    Nazwa = nazwa,
                    Opis = opis,
                    CzyAktywny = true
                };

                context.Rodzaj.Add(rodzaj);

                return rodzaj;
            }

            rodzaj.Opis = opis;
            rodzaj.CzyAktywny = true;

            return rodzaj;
        }

        private static async Task<Dictionary<string, Producent>> DodajLubAktualizujProducentow(FirmaContext context)
        {
            var wynik = new Dictionary<string, Producent>();

            wynik["Prusa Research"] = await Producent(
                context,
                "Prusa Research",
                "Czechy",
                "https://www.prusa3d.com",
                "Producent drukarek 3D i rozwiązań dla wymagających użytkowników.");

            wynik["Bambu Lab"] = await Producent(
                context,
                "Bambu Lab",
                "Chiny",
                "https://bambulab.com",
                "Producent szybkich drukarek 3D dla użytkowników domowych i firm.");

            wynik["Creality"] = await Producent(
                context,
                "Creality",
                "Chiny",
                "https://www.creality.com",
                "Popularne drukarki 3D i akcesoria w przystępnych cenach.");

            wynik["Fiberlogy"] = await Producent(
                context,
                "Fiberlogy",
                "Polska",
                "https://fiberlogy.com",
                "Polski producent filamentów do druku 3D.");

            wynik["Noctuo"] = await Producent(
                context,
                "Noctuo",
                "Polska",
                "https://example.com",
                "Akcesoria i elementy serwisowe dla drukarek 3D.");

            return wynik;
        }

        private static async Task<Producent> Producent(
            FirmaContext context,
            string nazwa,
            string kraj,
            string stronaWWW,
            string opis)
        {
            var producent = await context.Producent
                .FirstOrDefaultAsync(p => p.Nazwa == nazwa);

            if (producent == null)
            {
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

            producent.Kraj = kraj;
            producent.StronaWWW = stronaWWW;
            producent.Opis = opis;
            producent.CzyAktywny = true;

            return producent;
        }

        private static async Task DodajLubAktualizujTowary(
            FirmaContext context,
            Dictionary<string, Rodzaj> rodzaje,
            Dictionary<string, Producent> producenci)
        {
            await Towar(
                context,
                "PR3D-MK4",
                "Prusa MK4S",
                4199.00m,
                "Uniwersalna drukarka 3D do domu, edukacji i prototypowania. Dobry wybór dla osób, które chcą stabilnego sprzętu i wysokiej jakości wydruków.",
                Foto("Prusa MK4S"),
                rodzaje["Drukarki 3D"],
                producenci["Prusa Research"]);

            await Towar(
                context,
                "BBL-P1S",
                "Bambu Lab P1S",
                3799.00m,
                "Szybka drukarka 3D z obudową, przeznaczona do sprawnego drukowania modeli technicznych i użytkowych.",
                Foto("Bambu Lab P1S"),
                rodzaje["Drukarki 3D"],
                producenci["Bambu Lab"]);

            await Towar(
                context,
                "CRE-K1C",
                "Creality K1C",
                2699.00m,
                "Drukarka 3D do szybkiego druku i codziennej pracy z popularnymi materiałami.",
                Foto("Creality K1C"),
                rodzaje["Drukarki 3D"],
                producenci["Creality"]);

            await Towar(
                context,
                "FIL-PLA-MAT",
                "Filament PLA Matt 1 kg",
                89.90m,
                "Matowy filament PLA do wydruków dekoracyjnych, makiet i modeli użytkowych.",
                Foto("PLA Matt"),
                rodzaje["Filamenty"],
                producenci["Fiberlogy"]);

            await Towar(
                context,
                "FIL-PETG-CF",
                "Filament PETG CF 0.75 kg",
                159.90m,
                "Wzmocniony filament PETG z dodatkiem włókna węglowego do elementów technicznych.",
                Foto("PETG CF"),
                rodzaje["Filamenty"],
                producenci["Fiberlogy"]);

            await Towar(
                context,
                "DYSZA-04",
                "Dysza stalowa 0.4 mm",
                34.90m,
                "Dysza do drukarek 3D, odpowiednia do codziennego drukowania i materiałów technicznych.",
                Foto("Dysza 0.4"),
                rodzaje["Części"],
                producenci["Noctuo"]);

            await Towar(
                context,
                "PLYTA-PEI",
                "Płyta robocza PEI",
                119.00m,
                "Elastyczna płyta robocza PEI poprawiająca przyczepność pierwszej warstwy wydruku.",
                Foto("Płyta PEI"),
                rodzaje["Akcesoria"],
                producenci["Noctuo"]);

            await Towar(
                context,
                "ZEST-SERWIS",
                "Zestaw serwisowy drukarki 3D",
                149.00m,
                "Podstawowy zestaw narzędzi, dysz i elementów eksploatacyjnych do serwisowania drukarki 3D.",
                Foto("Zestaw serwisowy"),
                rodzaje["Serwis"],
                producenci["Noctuo"]);
        }

        private static async Task Towar(
            FirmaContext context,
            string kod,
            string nazwa,
            decimal cena,
            string opis,
            string fotoUrl,
            Rodzaj rodzaj,
            Producent producent)
        {
            var towar = await context.Towar
                .FirstOrDefaultAsync(t => t.Kod == kod);

            if (towar == null)
            {
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

                return;
            }

            towar.Nazwa = nazwa;
            towar.Cena = cena;
            towar.Opis = opis;
            towar.FotoUrl = fotoUrl;
            towar.IdRodzaju = rodzaj.IdRodzaju;
            towar.IdProducenta = producent.IdProducenta;
            towar.CzyAktywny = true;
        }

        private static async Task DodajLubAktualizujStanyMagazynowe(FirmaContext context)
        {
            await StanMagazynowy(context, "PR3D-MK4", 8, 2, "A1-01");
            await StanMagazynowy(context, "BBL-P1S", 5, 2, "A1-02");
            await StanMagazynowy(context, "CRE-K1C", 11, 3, "A1-03");
            await StanMagazynowy(context, "FIL-PLA-MAT", 64, 10, "B2-01");
            await StanMagazynowy(context, "FIL-PETG-CF", 28, 8, "B2-02");
            await StanMagazynowy(context, "DYSZA-04", 120, 20, "C3-01");
            await StanMagazynowy(context, "PLYTA-PEI", 24, 5, "C3-02");
            await StanMagazynowy(context, "ZEST-SERWIS", 16, 4, "C3-03");
        }

        private static async Task StanMagazynowy(
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

            var stan = await context.StanMagazynowy
                .FirstOrDefaultAsync(s => s.IdTowaru == towar.IdTowaru);

            if (stan == null)
            {
                context.StanMagazynowy.Add(new StanMagazynowy
                {
                    IdTowaru = towar.IdTowaru,
                    IloscSztuk = ilosc,
                    MinimalnaIlosc = minimum,
                    Lokalizacja = lokalizacja,
                    CzyAktywny = true
                });

                return;
            }

            stan.IloscSztuk = ilosc;
            stan.MinimalnaIlosc = minimum;
            stan.Lokalizacja = lokalizacja;
            stan.CzyAktywny = true;
        }

        private static async Task DodajLubAktualizujKlientowIZamowienia(FirmaContext context)
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
            else
            {
                klient.Imie = "Jan";
                klient.Nazwisko = "Kowalski";
                klient.Telefon = "600700800";
            }

            var drukarka = await context.Towar
                .FirstOrDefaultAsync(t => t.Kod == "PR3D-MK4");

            var filament = await context.Towar
                .FirstOrDefaultAsync(t => t.Kod == "FIL-PLA-MAT");

            if (drukarka == null || filament == null)
            {
                return;
            }

            var zamowienie = await context.Zamowienie
                .Include(z => z.PozycjaZamowienia)
                .FirstOrDefaultAsync(z => z.NumerZamowienia == "WWW-DEMO-001");

            if (zamowienie == null)
            {
                zamowienie = new Zamowienie
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
                    WartoscRazem = 0
                };

                context.Zamowienie.Add(zamowienie);
            }
            else
            {
                foreach (var pozycja in zamowienie.PozycjaZamowienia.ToList())
                {
                    context.Remove(pozycja);
                }

                zamowienie.PozycjaZamowienia.Clear();

                zamowienie.DataZamowienia = DateTime.Today.AddDays(-1);
                zamowienie.Status = "w trakcie";
                zamowienie.Ulica = "Kwiatowa";
                zamowienie.NumerDomu = "14";
                zamowienie.NumerLokalu = "3";
                zamowienie.KodPocztowy = "35-001";
                zamowienie.Miasto = "Rzeszów";
                zamowienie.IdKlienta = klient.IdKlienta;
            }

            zamowienie.WartoscRazem = drukarka.Cena + (2 * filament.Cena);

            zamowienie.PozycjaZamowienia.Add(new PozycjaZamowienia
            {
                IdTowaru = drukarka.IdTowaru,
                Ilosc = 1,
                CenaJednostkowa = drukarka.Cena
            });

            zamowienie.PozycjaZamowienia.Add(new PozycjaZamowienia
            {
                IdTowaru = filament.IdTowaru,
                Ilosc = 2,
                CenaJednostkowa = filament.Cena
            });
        }

        private static string Foto(string tekst)
        {
            return $"https://placehold.co/900x700/png?text={Uri.EscapeDataString(tekst)}";
        }
    }
}