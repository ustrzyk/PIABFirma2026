using ClosedXML.Excel;
using Firma.Data.Data.Sklep;

namespace Firma.Intranet.Services.Dokumenty
{
    public class ZamowienieExcelGenerator
    {
        public byte[] Generuj(IList<Zamowienie> zamowienia)
        {
            using var workbook = new XLWorkbook();

            DodajArkuszZamowien(workbook, zamowienia);
            DodajArkuszPozycji(workbook, zamowienia);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        private static void DodajArkuszZamowien(XLWorkbook workbook, IList<Zamowienie> zamowienia)
        {
            var worksheet = workbook.Worksheets.Add("Zamówienia");

            worksheet.Cell(1, 1).Value = "Numer zamówienia";
            worksheet.Cell(1, 2).Value = "Data zamówienia";
            worksheet.Cell(1, 3).Value = "Status";
            worksheet.Cell(1, 4).Value = "Wartość razem";
            worksheet.Cell(1, 5).Value = "Klient";
            worksheet.Cell(1, 6).Value = "Email";
            worksheet.Cell(1, 7).Value = "Telefon";
            worksheet.Cell(1, 8).Value = "Ulica";
            worksheet.Cell(1, 9).Value = "Numer domu";
            worksheet.Cell(1, 10).Value = "Numer lokalu";
            worksheet.Cell(1, 11).Value = "Kod pocztowy";
            worksheet.Cell(1, 12).Value = "Miasto";
            worksheet.Cell(1, 13).Value = "Liczba pozycji";

            var row = 2;

            foreach (var zamowienie in zamowienia)
            {
                worksheet.Cell(row, 1).Value = zamowienie.NumerZamowienia;

                if (zamowienie.DataZamowienia != null)
                {
                    worksheet.Cell(row, 2).Value = zamowienie.DataZamowienia.Value;
                }

                worksheet.Cell(row, 3).Value = zamowienie.Status;
                worksheet.Cell(row, 4).Value = zamowienie.WartoscRazem;

                if (zamowienie.Klient != null)
                {
                    worksheet.Cell(row, 5).Value = $"{zamowienie.Klient.Imie} {zamowienie.Klient.Nazwisko}";
                    worksheet.Cell(row, 6).Value = zamowienie.Klient.Email;
                    worksheet.Cell(row, 7).Value = zamowienie.Klient.Telefon;
                }
                else
                {
                    worksheet.Cell(row, 5).Value = "brak klienta";
                }

                worksheet.Cell(row, 8).Value = zamowienie.Ulica;
                worksheet.Cell(row, 9).Value = zamowienie.NumerDomu;
                worksheet.Cell(row, 10).Value = zamowienie.NumerLokalu;
                worksheet.Cell(row, 11).Value = zamowienie.KodPocztowy;
                worksheet.Cell(row, 12).Value = zamowienie.Miasto;
                worksheet.Cell(row, 13).Value = zamowienie.PozycjaZamowienia?.Count ?? 0;

                row++;
            }

            UstawStylTabeli(worksheet, 13);
            worksheet.Column(2).Style.DateFormat.Format = "dd.mm.yyyy";
            worksheet.Column(4).Style.NumberFormat.Format = "#,##0.00 zł";
        }

        private static void DodajArkuszPozycji(XLWorkbook workbook, IList<Zamowienie> zamowienia)
        {
            var worksheet = workbook.Worksheets.Add("Pozycje");

            worksheet.Cell(1, 1).Value = "Numer zamówienia";
            worksheet.Cell(1, 2).Value = "Kod towaru";
            worksheet.Cell(1, 3).Value = "Towar";
            worksheet.Cell(1, 4).Value = "Ilość";
            worksheet.Cell(1, 5).Value = "Cena jednostkowa";
            worksheet.Cell(1, 6).Value = "Wartość pozycji";

            var row = 2;

            foreach (var zamowienie in zamowienia)
            {
                if (zamowienie.PozycjaZamowienia == null || !zamowienie.PozycjaZamowienia.Any())
                {
                    worksheet.Cell(row, 1).Value = zamowienie.NumerZamowienia;
                    worksheet.Cell(row, 3).Value = "brak pozycji";
                    row++;

                    continue;
                }

                foreach (var pozycja in zamowienie.PozycjaZamowienia.OrderBy(p => p.IdPozycjiZamowienia))
                {
                    worksheet.Cell(row, 1).Value = zamowienie.NumerZamowienia;

                    if (pozycja.Towar != null)
                    {
                        worksheet.Cell(row, 2).Value = pozycja.Towar.Kod;
                        worksheet.Cell(row, 3).Value = pozycja.Towar.Nazwa;
                    }
                    else
                    {
                        worksheet.Cell(row, 2).Value = pozycja.IdTowaru;
                        worksheet.Cell(row, 3).Value = "brak danych towaru";
                    }

                    worksheet.Cell(row, 4).Value = pozycja.Ilosc;
                    worksheet.Cell(row, 5).Value = pozycja.CenaJednostkowa;
                    worksheet.Cell(row, 6).Value = pozycja.Ilosc * pozycja.CenaJednostkowa;

                    row++;
                }
            }

            UstawStylTabeli(worksheet, 6);
            worksheet.Column(5).Style.NumberFormat.Format = "#,##0.00 zł";
            worksheet.Column(6).Style.NumberFormat.Format = "#,##0.00 zł";
        }

        private static void UstawStylTabeli(IXLWorksheet worksheet, int liczbaKolumn)
        {
            var ostatniWiersz = worksheet.LastRowUsed()?.RowNumber() ?? 1;
            var zakres = worksheet.Range(1, 1, ostatniWiersz, liczbaKolumn);

            zakres.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            zakres.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            var naglowek = worksheet.Range(1, 1, 1, liczbaKolumn);
            naglowek.Style.Font.Bold = true;
            naglowek.Style.Fill.BackgroundColor = XLColor.LightGray;

            worksheet.Columns().AdjustToContents();
            worksheet.SheetView.FreezeRows(1);
        }
    }
}