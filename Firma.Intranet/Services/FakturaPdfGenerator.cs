using Firma.Data.Data.Sklep;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace Firma.Intranet.Services
{
    public class FakturaPdfGenerator
    {
        public byte[] Generuj(Zamowienie zamowienie)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var kultura = new CultureInfo("pl-PL");
            var pozycje = zamowienie.PozycjaZamowienia
                .OrderBy(p => p.IdPozycjiZamowienia)
                .ToList();

            var numerFaktury = $"FV/{zamowienie.NumerZamowienia}";
            var dataWystawienia = DateTime.Now;
            var dataZamowienia = zamowienie.DataZamowienia ?? DateTime.Now;

            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(35);
                    page.DefaultTextStyle(x => x.FontSize(10).FontFamily("Arial"));

                    page.Header().Element(header =>
                    {
                        header.Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text("FAKTURA").FontSize(24).Bold();
                                column.Item().Text(numerFaktury).FontSize(13).SemiBold();
                            });

                            row.ConstantItem(210).Column(column =>
                            {
                                column.Item().AlignRight().Text($"Data wystawienia: {dataWystawienia:dd.MM.yyyy}");
                                column.Item().AlignRight().Text($"Data zamówienia: {dataZamowienia:dd.MM.yyyy}");
                                column.Item().AlignRight().Text($"Status: {zamowienie.Status}");
                            });
                        });
                    });

                    page.Content().PaddingVertical(20).Column(column =>
                    {
                        column.Spacing(16);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Element(box =>
                            {
                                box.Border(1)
                                   .BorderColor(Colors.Grey.Lighten2)
                                   .Padding(10)
                                   .Column(k =>
                                   {
                                       k.Item().Text("Sprzedawca").Bold();
                                       k.Item().Text("Sklep 3D");
                                       k.Item().Text("Projekt studencki PIABFirma2026");
                                       k.Item().Text("E-mail: kontakt@sklep3d.pl");
                                   });
                            });

                            row.ConstantItem(20);

                            row.RelativeItem().Element(box =>
                            {
                                box.Border(1)
                                   .BorderColor(Colors.Grey.Lighten2)
                                   .Padding(10)
                                   .Column(k =>
                                   {
                                       k.Item().Text("Nabywca").Bold();

                                       if (zamowienie.Klient != null)
                                       {
                                           k.Item().Text($"{zamowienie.Klient.Imie} {zamowienie.Klient.Nazwisko}");
                                           k.Item().Text($"E-mail: {zamowienie.Klient.Email}");

                                           if (!string.IsNullOrWhiteSpace(zamowienie.Klient.Telefon))
                                           {
                                               k.Item().Text($"Telefon: {zamowienie.Klient.Telefon}");
                                           }
                                       }
                                       else
                                       {
                                           k.Item().Text("Brak danych klienta");
                                       }

                                       k.Item().PaddingTop(6).Text("Adres dostawy").Bold();
                                       k.Item().Text(PelnyAdres(zamowienie));
                                   });
                            });
                        });

                        column.Item().Text("Pozycje dokumentu").FontSize(14).Bold();

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(30);
                                columns.RelativeColumn(3);
                                columns.ConstantColumn(55);
                                columns.ConstantColumn(75);
                                columns.ConstantColumn(80);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(KomorkaNaglowka).Text("Lp.");
                                header.Cell().Element(KomorkaNaglowka).Text("Towar");
                                header.Cell().Element(KomorkaNaglowka).AlignRight().Text("Ilość");
                                header.Cell().Element(KomorkaNaglowka).AlignRight().Text("Cena");
                                header.Cell().Element(KomorkaNaglowka).AlignRight().Text("Wartość");
                            });

                            if (pozycje.Any())
                            {
                                var lp = 1;

                                foreach (var pozycja in pozycje)
                                {
                                    var wartosc = pozycja.Ilosc * pozycja.CenaJednostkowa;
                                    var nazwaTowaru = pozycja.Towar != null
                                        ? $"{pozycja.Towar.Nazwa} ({pozycja.Towar.Kod})"
                                        : $"Towar ID: {pozycja.IdTowaru}";

                                    table.Cell().Element(Komorka).Text(lp.ToString());
                                    table.Cell().Element(Komorka).Text(nazwaTowaru);
                                    table.Cell().Element(Komorka).AlignRight().Text(pozycja.Ilosc.ToString());
                                    table.Cell().Element(Komorka).AlignRight().Text(pozycja.CenaJednostkowa.ToString("C", kultura));
                                    table.Cell().Element(Komorka).AlignRight().Text(wartosc.ToString("C", kultura));

                                    lp++;
                                }
                            }
                            else
                            {
                                table.Cell().ColumnSpan(5).Element(Komorka).Text("Brak pozycji zamówienia");
                            }
                        });

                        column.Item().AlignRight().Column(podsumowanie =>
                        {
                            podsumowanie.Item().Text($"Razem do zapłaty: {zamowienie.WartoscRazem.ToString("C", kultura)}")
                                .FontSize(14)
                                .Bold();

                            podsumowanie.Item().Text("Kwota według wartości zapisanej przy zamówieniu")
                                .FontSize(9)
                                .FontColor(Colors.Grey.Darken1);
                        });

                        column.Item().PaddingTop(10).Text("Uwagi").Bold();
                        column.Item().Text("Dokument wygenerowany automatycznie w panelu Intranet.");
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Strona ");
                        text.CurrentPageNumber();
                        text.Span(" / ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();
        }

        private static string PelnyAdres(Zamowienie zamowienie)
        {
            var lokal = string.IsNullOrWhiteSpace(zamowienie.NumerLokalu)
                ? string.Empty
                : $"/{zamowienie.NumerLokalu}";

            return $"{zamowienie.Ulica} {zamowienie.NumerDomu}{lokal}, {zamowienie.KodPocztowy} {zamowienie.Miasto}";
        }

        private static IContainer KomorkaNaglowka(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten1)
                .Background(Colors.Grey.Lighten3)
                .Padding(5);
        }

        private static IContainer Komorka(IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(5);
        }
    }
}