# PIABFirma2026

Projekt składa się z dwóch aplikacji ASP.NET Core MVC:

* **Firma.Intranet** — panel administracyjny dla pracowników i administratora.
* **Firma.PortalWWW** — publiczny portal sklepu internetowego.

Projekt wykorzystuje wspólną bazę danych, wspólne modele encji oraz osobne projekty serwisowe. Dzięki temu kontrolery są krótsze i odpowiadają głównie za obsługę żądań HTTP, a logika biznesowa znajduje się w serwisach.

## Technologie

* .NET 10
* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* Bootstrap
* ClosedXML
* QuestPDF

## Struktura rozwiązania

```text
PIABFirma2026
├── Firma.Data
├── Firma.Interfaces
├── Firma.Services
├── Firma.Services.Data
├── Firma.Intranet
├── Firma.Intranet.Interfaces
├── Firma.Intranet.Services
├── Firma.Intranet.Services.Data
├── Firma.PortalWWW
└── Firma.slnx
```

## Opis projektów

### Firma.Data

Projekt zawiera modele danych oraz kontekst bazy danych.

Najważniejsze elementy:

* `FirmaContext` — główny kontekst Entity Framework Core.
* Modele sklepu:

  * `Towar`
  * `Rodzaj`
  * `Producent`
  * `Klient`
  * `Zamowienie`
  * `PozycjaZamowienia`
  * `StanMagazynowy`
  * `ZalacznikTowaru`
* Modele CMS:

  * `Aktualnosc`
  * `Promocja`
  * `Strona`
  * `UstawieniePortalu`

### Firma.Intranet

Panel administracyjny dla pracowników i administratora.

Najważniejsze funkcje:

* logowanie i wylogowanie użytkowników,
* obsługa ról użytkowników,
* zarządzanie użytkownikami,
* obsługa towarów,
* obsługa producentów,
* obsługa rodzajów,
* obsługa stanów magazynowych,
* obsługa klientów,
* obsługa zamówień,
* obsługa pozycji zamówień,
* obsługa załączników towarów,
* obsługa aktualności,
* obsługa promocji,
* obsługa stron CMS,
* obsługa ustawień portalu,
* eksport faktur PDF,
* eksport zamówień do Excela,
* import zamówień z Excela.

### Firma.Intranet.Interfaces

Projekt zawiera interfejsy serwisów wykorzystywanych przez panel Intranet.

Przykładowe interfejsy:

* `IZamowienieIntranetService`
* `ITowarIntranetService`
* `IKlientIntranetService`
* `IProducentIntranetService`
* `IRodzajIntranetService`
* `IStanMagazynowyIntranetService`
* `IUzytkownikIntranetService`
* `IKontoIntranetService`

### Firma.Intranet.Services

Projekt zawiera implementacje serwisów dla panelu Intranet.

Najważniejsze elementy:

* serwisy CRUD dla modułów administracyjnych,
* obsługa aktywacji i dezaktywacji rekordów,
* bezpieczne usuwanie rekordów powiązanych z innymi danymi,
* obsługa użytkowników i ról,
* obsługa logowania,
* generowanie faktur PDF,
* eksport zamówień do plików Excel,
* import zamówień z pliku Excel,
* seeder użytkownika startowego.

### Firma.Intranet.Services.Data

Projekt zawiera klasy DTO i modele pomocnicze używane przez serwisy Intranetu.

Przykładowe klasy:

* `KlientSelectItemDto`
* `TowarSelectItemDto`
* `ProducentSelectItemDto`
* `RodzajSelectItemDto`
* `UzytkownikListaItemDto`
* `UzytkownikEdycjaDto`
* `UzytkownikUsuniecieDto`
* `OperacjaUzytkownikaWynikDto`
* `ImportZamowienExcelWynikDto`
* `LogowanieWynikDto`

### Firma.PortalWWW

Publiczna część sklepu internetowego.

Najważniejsze funkcje:

* strona główna portalu,
* lista towarów,
* szczegóły towaru,
* menu rodzajów produktów,
* aktualności,
* promocje,
* producenci,
* stany magazynowe,
* strony CMS,
* ustawienia wyglądu portalu pobierane z bazy,
* koszyk klienta,
* składanie zamówień,
* potwierdzenie zamówienia,
* sprawdzanie statusu zamówienia bez logowania,
* rejestracja klienta,
* logowanie klienta,
* panel klienta,
* edycja danych i adresu klienta,
* lista zamówień klienta,
* szczegóły i status zamówienia w panelu klienta.

### Firma.Interfaces

Projekt zawiera interfejsy serwisów używanych przez publiczny portal.

Przykładowe obszary:

* obsługa sklepu,
* obsługa koszyka i zamówień publicznych,
* obsługa konta klienta,
* obsługa CMS,
* obsługa ustawień portalu.

### Firma.Services

Projekt zawiera implementacje serwisów używanych przez publiczny portal.

Najważniejsze elementy:

* pobieranie aktywnych danych do portalu,
* obsługa list produktów i szczegółów produktu,
* obsługa koszyka,
* obsługa składania zamówienia,
* obsługa statusu zamówienia,
* obsługa konta klienta,
* obsługa danych CMS.

### Firma.Services.Data

Projekt zawiera DTO używane w publicznym portalu.

Przykładowe obszary DTO:

* towary,
* rodzaje,
* producenci,
* promocje,
* aktualności,
* zamówienia publiczne,
* konto klienta,
* ustawienia portalu.

## Baza danych

Projekt używa SQL Server oraz Entity Framework Core.

Połączenie do bazy jest konfigurowane w plikach:

```text
Firma.Intranet/appsettings.json
Firma.PortalWWW/appsettings.json
```

Wymagany connection string:

```json
{
  "ConnectionStrings": {
    "FirmaContext": "Server=(localdb)\\mssqllocaldb;Database=PIABFirma2026;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

Nazwa bazy danych może być inna, ale connection string musi mieć nazwę `FirmaContext`.

## Uruchomienie projektu

### 1. Przywrócenie paczek

W głównym katalogu rozwiązania uruchom:

```powershell
dotnet restore
```

### 2. Budowanie rozwiązania

```powershell
dotnet build
```

### 3. Aktualizacja bazy danych

Migracje znajdują się w projekcie `Firma.Intranet`.

```powershell
dotnet ef database update --project Firma.Intranet
```

Jeżeli narzędzie `dotnet ef` nie jest zainstalowane, można je zainstalować poleceniem:

```powershell
dotnet tool install --global dotnet-ef
```

### 4. Uruchomienie Intranetu

```powershell
dotnet run --project Firma.Intranet
```

### 5. Uruchomienie portalu publicznego

W drugim oknie terminala:

```powershell
dotnet run --project Firma.PortalWWW
```

## Konto startowe

Po uruchomieniu aplikacji Intranet tworzony jest użytkownik startowy:

```text
E-mail: tsaran@test.pl
Hasło: Tsaran123!
Rola: Administrator
```

Seeder tworzy też role:

```text
Administrator
Pracownik
```

## Role i uprawnienia

### Administrator

Administrator ma dostęp do pełnej administracji, w tym:

* zarządzania użytkownikami,
* usuwania rekordów,
* importu zamówień z Excela,
* masowych akcji,
* aktywacji i dezaktywacji rekordów.

### Pracownik

Pracownik ma dostęp do podstawowej obsługi panelu, ale nie widzi akcji administracyjnych takich jak usuwanie, import lub zarządzanie użytkownikami.

## Aktywacja i dezaktywacja danych

W projekcie wiele rekordów można aktywować albo dezaktywować zamiast usuwać.

Dotyczy to między innymi:

* towarów,
* producentów,
* rodzajów,
* stanów magazynowych,
* aktualności,
* promocji,
* stron CMS,
* ustawień portalu.

Portal publiczny pokazuje tylko aktywne dane.

## Bezpieczne usuwanie

Niektóre rekordy są usuwane bezpośrednio tylko wtedy, gdy nie mają ważnych powiązań.

Przykłady:

* towar użyty w zamówieniu nie jest usuwany, tylko dezaktywowany,
* producent mający towary jest dezaktywowany,
* rodzaj mający towary jest dezaktywowany,
* klient mający zamówienia nie może zostać usunięty.

## Portal publiczny

Portal publiczny pozwala klientowi przeglądać ofertę sklepu i składać zamówienia.

Najważniejsze części portalu:

* strona główna,
* sklep,
* szczegóły produktu,
* promocje,
* aktualności,
* producenci,
* dostępność towarów,
* koszyk,
* formularz zamówienia,
* potwierdzenie zamówienia,
* status zamówienia,
* konto klienta.

## Koszyk i zamówienia

Klient może:

* dodać produkt do koszyka,
* zmienić ilość produktu,
* usunąć produkt z koszyka,
* wyczyścić koszyk,
* przejść do formularza zamówienia,
* złożyć zamówienie.

Zamówienie można złożyć jako:

* klient niezalogowany,
* klient zalogowany.

Po złożeniu zamówienia klient otrzymuje numer zamówienia.

## Status zamówienia

Status zamówienia jest dostępny na dwa sposoby:

### 1. Status bez logowania

Klient może wejść w stronę `Status zamówienia` i podać:

* numer zamówienia,
* adres e-mail użyty przy zamówieniu.

Ta opcja jest przydatna dla klientów, którzy złożyli zamówienie bez konta.

### 2. Status w panelu klienta

Zalogowany klient może wejść w:

```text
Moje konto -> Moje zamówienia -> Szczegóły i status
```

W panelu klient widzi swoje zamówienia oraz ich aktualne statusy.

## Konto klienta

Portal publiczny posiada prosty panel klienta.

Klient może:

* zarejestrować konto,
* zalogować się,
* wylogować się,
* edytować dane kontaktowe,
* edytować adres dostawy,
* przeglądać swoje zamówienia,
* sprawdzać szczegóły zamówienia,
* sprawdzać status zamówienia.

Loginem klienta jest adres e-mail.

Wymagania hasła:

```text
Minimum 8 znaków, mała i duża litera, cyfra oraz znak specjalny.
```

## Intranet

Intranet służy do administracyjnej obsługi sklepu.

Najważniejsze moduły:

* towary,
* producenci,
* rodzaje,
* stany magazynowe,
* klienci,
* zamówienia,
* pozycje zamówień,
* załączniki towarów,
* aktualności,
* promocje,
* strony CMS,
* ustawienia portalu,
* użytkownicy.

## Obsługa zamówień w Intranecie

Pracownik lub administrator może:

* przeglądać zamówienia,
* sprawdzać szczegóły zamówienia,
* zmieniać status zamówienia,
* eksportować zamówienia do Excela,
* importować zamówienia z Excela,
* generować faktury PDF.

Zmiana statusu w Intranecie jest widoczna w portalu publicznym.

## Dokumenty PDF i Excel

Panel Intranet obsługuje:

* generowanie faktury PDF dla pojedynczego zamówienia,
* generowanie paczki ZIP z fakturami PDF dla zaznaczonych zamówień,
* eksport wszystkich zamówień do Excela,
* eksport zaznaczonych zamówień do Excela,
* pobieranie szablonu importu zamówień,
* import zamówień z pliku `.xlsx`.

Za PDF odpowiada biblioteka QuestPDF.

Za Excel odpowiada biblioteka ClosedXML.

## Import zamówień z Excela

Import obsługuje plik `.xlsx` z arkuszem `Import`.

Wymagane kolumny:

```text
NumerZamowienia
DataZamowienia
Status
WartoscRazem
EmailKlienta
ImieKlienta
NazwiskoKlienta
TelefonKlienta
Ulica
NumerDomu
NumerLokalu
KodPocztowy
Miasto
```

Import tworzy zamówienia bez pozycji zamówienia.

Jeżeli klient o podanym adresie e-mail już istnieje, zostanie użyty istniejący klient.

Jeżeli klient nie istnieje, zostanie utworzony nowy klient.

## Załączniki towarów

Towary mogą mieć załączniki widoczne w panelu Intranet i portalu publicznym.

Dozwolone rozszerzenia:

```text
.pdf
.doc
.docx
.xls
.xlsx
.txt
.png
.jpg
.jpeg
.webp
```

Maksymalny rozmiar pliku:

```text
10 MB
```

## Ustawienia portalu

Wygląd portalu publicznego jest sterowany ustawieniami zapisanymi w bazie danych.

Przykładowe ustawienia:

* nazwa portalu,
* kolor główny,
* kolor tła,
* kolor nawigacji,
* kolor stopki,
* kolor przycisków,
* tekst stopki,
* dane kontaktowe,
* widoczność elementów portalu.

## Migracje

Migracje znajdują się w projekcie `Firma.Intranet`.

Najczęstsze polecenia:

```powershell
dotnet ef migrations add NazwaMigracji --project Firma.Intranet
dotnet ef database update --project Firma.Intranet
```

## Budowanie całego rozwiązania

```powershell
dotnet restore
dotnet build
```

Build powinien zakończyć się bez błędów.

## Testy ręczne po uruchomieniu

Po uruchomieniu projektu warto sprawdzić:

* logowanie do Intranetu,
* listę towarów,
* listę zamówień,
* zmianę statusu zamówienia,
* stronę główną portalu,
* sklep,
* szczegóły produktu,
* koszyk,
* formularz zamówienia,
* potwierdzenie zamówienia,
* status zamówienia bez logowania,
* rejestrację klienta,
* logowanie klienta,
* panel klienta,
* edycję danych klienta,
* szczegóły zamówienia w panelu klienta.

## Licencja

Projekt zawiera plik `LICENSE`.
