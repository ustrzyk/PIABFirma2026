using ClosedXML.Excel;

namespace Firma.Intranet.Services
{
    public class ZamowienieExcelSzablonGenerator
    {
        public byte[] Generuj()
        {
            using var workbook = new XLWorkbook();

            DodajArkuszImportu(workbook);
            DodajArkuszPrzykladu(workbook);
            DodajArkuszInstrukcji(workbook);

            using var stream = new MemoryStream();
            workbook.SaveAs(stream);

            return stream.ToArray();
        }

        private static void DodajArkuszImportu(XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Import");

            UstawNaglowki(worksheet);
            UstawStyl(worksheet, 13);
        }

        private static void DodajArkuszPrzykladu(XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Przyklad");

            UstawNaglowki(worksheet);

            worksheet.Cell(2, 1).Value = "ZAM/2026/001";
            worksheet.Cell(2, 2).Value = DateTime.Today;
            worksheet.Cell(2, 3).Value = "Nowe";
            worksheet.Cell(2, 4).Value = 2500.00m;
            worksheet.Cell(2, 5).Value = "klient@example.com";
            worksheet.Cell(2, 6).Value = "Jan";
            worksheet.Cell(2, 7).Value = "Kowalski";
            worksheet.Cell(2, 8).Value = "500600700";
            worksheet.Cell(2, 9).Value = "Przykładowa";
            worksheet.Cell(2, 10).Value = "10";
            worksheet.Cell(2, 11).Value = "5";
            worksheet.Cell(2, 12).Value = "30-001";
            worksheet.Cell(2, 13).Value = "Kraków";

            UstawStyl(worksheet, 13);
            worksheet.Column(2).Style.DateFormat.Format = "dd.mm.yyyy";
            worksheet.Column(4).Style.NumberFormat.Format = "#,##0.00";
        }

        private static void DodajArkuszInstrukcji(XLWorkbook workbook)
        {
            var worksheet = workbook.Worksheets.Add("Instrukcja");

            worksheet.Cell(1, 1).Value = "Instrukcja importu zamówień";
            worksheet.Cell(3, 1).Value = "1. Dane do importu wpisz w arkuszu Import od wiersza 2.";
            worksheet.Cell(4, 1).Value = "2. Nie zmieniaj nazw kolumn.";
            worksheet.Cell(5, 1).Value = "3. Numer zamówienia musi być unikalny.";
            worksheet.Cell(6, 1).Value = "4. Jeżeli klient o podanym e-mailu istnieje, zostanie użyty istniejący klient.";
            worksheet.Cell(7, 1).Value = "5. Jeżeli klient nie istnieje, zostanie utworzony nowy klient.";
            worksheet.Cell(8, 1).Value = "6. Import tworzy zamówienia bez pozycji zamówienia.";

            worksheet.Cell(10, 1).Value = "Wymagane kolumny:";
            worksheet.Cell(11, 1).Value = "NumerZamowienia, DataZamowienia, Status, WartoscRazem, EmailKlienta, ImieKlienta, NazwiskoKlienta, Ulica, NumerDomu, KodPocztowy, Miasto";

            worksheet.Range(1, 1, 1, 1).Style.Font.Bold = true;
            worksheet.Range(1, 1, 1, 1).Style.Font.FontSize = 16;
            worksheet.Columns().AdjustToContents();
        }

        private static void UstawNaglowki(IXLWorksheet worksheet)
        {
            worksheet.Cell(1, 1).Value = "NumerZamowienia";
            worksheet.Cell(1, 2).Value = "DataZamowienia";
            worksheet.Cell(1, 3).Value = "Status";
            worksheet.Cell(1, 4).Value = "WartoscRazem";
            worksheet.Cell(1, 5).Value = "EmailKlienta";
            worksheet.Cell(1, 6).Value = "ImieKlienta";
            worksheet.Cell(1, 7).Value = "NazwiskoKlienta";
            worksheet.Cell(1, 8).Value = "TelefonKlienta";
            worksheet.Cell(1, 9).Value = "Ulica";
            worksheet.Cell(1, 10).Value = "NumerDomu";
            worksheet.Cell(1, 11).Value = "NumerLokalu";
            worksheet.Cell(1, 12).Value = "KodPocztowy";
            worksheet.Cell(1, 13).Value = "Miasto";
        }

        private static void UstawStyl(IXLWorksheet worksheet, int liczbaKolumn)
        {
            var zakres = worksheet.Range(1, 1, 1, liczbaKolumn);

            zakres.Style.Font.Bold = true;
            zakres.Style.Fill.BackgroundColor = XLColor.LightGray;
            zakres.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            zakres.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

            worksheet.SheetView.FreezeRows(1);
            worksheet.Columns().AdjustToContents();
        }
    }
}