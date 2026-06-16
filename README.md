\# PIABFirma2026



Projekt składa się z dwóch aplikacji ASP.NET Core MVC:



\* \*\*Firma.Intranet\*\* — panel administracyjny do zarządzania sklepem.

\* \*\*Firma.PortalWWW\*\* — publiczny portal sklepu internetowego.



Projekt wykorzystuje wspólną bazę danych i wspólne modele encji w projekcie \*\*Firma.Data\*\*. Logika biznesowa została rozdzielona do osobnych projektów usługowych, żeby kontrolery były krótsze i odpowiadały głównie za obsługę żądań HTTP.



\## Technologie



\* .NET 10

\* ASP.NET Core MVC

\* Entity Framework Core

\* SQL Server

\* ASP.NET Core Identity

\* Bootstrap

\* ClosedXML

\* QuestPDF



\## Struktura rozwiązania



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



\## Opis projektów



\### Firma.Data



Projekt zawiera modele danych oraz kontekst bazy danych.



Najważniejsze elementy:



\* `FirmaContext` — główny kontekst Entity Framework Core.

\* Modele sklepu:



&#x20; \* `Towar`

&#x20; \* `Rodzaj`

&#x20; \* `Producent`

&#x20; \* `Klient`

&#x20; \* `Zamowienie`

&#x20; \* `PozycjaZamowienia`

&#x20; \* `StanMagazynowy`

&#x20; \* `ZalacznikTowaru`

\* Modele CMS:



&#x20; \* `Aktualnosc`

&#x20; \* `Promocja`

&#x20; \* `Strona`

&#x20; \* `UstawieniePortalu`



\### Firma.Intranet



Panel administracyjny dla pracowników i administratora.



Najważniejsze funkcje:



\* logowanie i wylogowanie,

\* role użytkowników,

\* zarządzanie użytkownikami,

\* obsługa towarów,

\* obsługa producentów,

\* obsługa rodzajów,

\* obsługa stanów magazynowych,

\* obsługa klientów,

\* obsługa zamówień,

\* obsługa pozycji zamówień,

\* obsługa załączników towarów,

\* obsługa aktualności,

\* obsługa promocji,

\* obsługa stron CMS,

\* obsługa ustawień portalu,

\* eksport faktur PDF,

\* eksport zamówień do Excela,

\* import zamówień z Excela.



\### Firma.Intranet.Interfaces



Projekt zawiera interfejsy serwisów wykorzystywanych przez panel Intranet.



Przykładowe interfejsy:



\* `IZamowienieIntranetService`

\* `ITowarIntranetService`

\* `IKlientIntranetService`

\* `IProducentIntranetService`

\* `IRodzajIntranetService`

\* `IStanMagazynowyIntranetService`

\* `IUzytkownikIntranetService`

\* `IKontoIntranetService`



\### Firma.Intranet.Services



Projekt zawiera implementacje serwisów dla panelu Intranet.



Najważniejsze elementy:



\* serwisy CRUD dla modułów administracyjnych,

\* logika aktywacji i dezaktywacji rekordów,

\* bezpieczne usuwanie rekordów powiązanych z innymi danymi,

\* obsługa użytkowników i ról,

\* obsługa logowania,

\* generowanie faktur PDF,

\* eksport zamówień do plików Excel,

\* import zamówień z pliku Excel,

\* seeder użytkownika startowego.



\### Firma.Intranet.Services.Data



Projekt zawiera klasy DTO i modele pomocnicze używane przez serwisy Intranetu.



Przykładowe klasy:



\* `KlientSelectItemDto`

\* `TowarSelectItemDto`

\* `ProducentSelectItemDto`

\* `RodzajSelectItemDto`

\* `UzytkownikListaItemDto`

\* `UzytkownikEdycjaDto`

\* `UzytkownikUsuniecieDto`

\* `OperacjaUzytkownikaWynikDto`

\* `ImportZamowienExcelWynikDto`

\* `LogowanieWynikDto`



\### Firma.PortalWWW



Publiczna część sklepu.



Najważniejsze funkcje:



\* strona główna,

\* lista towarów,

\* szczegóły towaru,

\* menu rodzajów,

\* aktualności,

\* promocje,

\* strony CMS,

\* ustawienia wyglądu portalu pobierane z bazy,

\* wyświetlanie aktywnych danych publicznych.



\### Firma.Interfaces



Projekt zawiera interfejsy serwisów używanych przez publiczny portal.



\### Firma.Services



Projekt zawiera implementacje serwisów używanych przez publiczny portal.



\### Firma.Services.Data



Projekt zawiera DTO używane w publicznym portalu.



\## Baza danych



Projekt używa SQL Server oraz Entity Framework Core.



Połączenie do bazy jest konfigurowane w plikach `appsettings.json` projektów:



```text

Firma.Intranet/appsettings.json

Firma.PortalWWW/appsettings.json

```



Wymagany connection string:



```json

{

&#x20; "ConnectionStrings": {

&#x20;   "FirmaContext": "Server=(localdb)\\\\mssqllocaldb;Database=PIABFirma2026;Trusted\_Connection=True;MultipleActiveResultSets=true"

&#x20; }

}

```



Nazwa bazy danych może być inna, ale connection string musi mieć nazwę `FirmaContext`.



\## Uruchomienie projektu



\### 1. Przywrócenie paczek



W głównym katalogu rozwiązania uruchom:



```powershell

dotnet restore

```



\### 2. Budowanie projektu



```powershell

dotnet build

```



\### 3. Aktualizacja bazy danych



Dla panelu Intranet:



```powershell

dotnet ef database update --project Firma.Intranet

```



Jeżeli narzędzie `dotnet ef` nie jest zainstalowane, można je zainstalować poleceniem:



```powershell

dotnet tool install --global dotnet-ef

```



\### 4. Uruchomienie Intranetu



```powershell

dotnet run --project Firma.Intranet

```



\### 5. Uruchomienie portalu publicznego



W drugim oknie terminala:



```powershell

dotnet run --project Firma.PortalWWW

```



\## Konto startowe



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



\## Role i uprawnienia



\### Administrator



Administrator ma dostęp do pełnej administracji, w tym:



\* zarządzania użytkownikami,

\* usuwania rekordów,

\* importu zamówień z Excela,

\* masowych akcji,

\* aktywacji i dezaktywacji rekordów.



\### Pracownik



Pracownik ma dostęp do podstawowej obsługi panelu, ale nie widzi akcji administracyjnych takich jak usuwanie, import lub zarządzanie użytkownikami.



\## Aktywacja i dezaktywacja danych



W projekcie wiele rekordów można aktywować lub dezaktywować zamiast usuwać.



Dotyczy to między innymi:



\* towarów,

\* producentów,

\* rodzajów,

\* stanów magazynowych,

\* aktualności,

\* promocji,

\* stron CMS,

\* ustawień portalu.



Portal publiczny pokazuje tylko aktywne dane.



\## Bezpieczne usuwanie



Niektóre rekordy są usuwane bezpośrednio tylko wtedy, gdy nie mają ważnych powiązań.



Przykłady:



\* towar użyty w zamówieniu nie jest usuwany, tylko dezaktywowany,

\* producent mający towary jest dezaktywowany,

\* rodzaj mający towary jest dezaktywowany,

\* klient mający zamówienia nie może zostać usunięty.



\## Dokumenty PDF i Excel



Panel Intranet obsługuje:



\* generowanie faktury PDF dla pojedynczego zamówienia,

\* generowanie paczki ZIP z fakturami PDF dla zaznaczonych zamówień,

\* eksport wszystkich zamówień do Excela,

\* eksport zaznaczonych zamówień do Excela,

\* pobieranie szablonu importu zamówień,

\* import zamówień z pliku `.xlsx`.



Za PDF odpowiada biblioteka QuestPDF.



Za Excel odpowiada biblioteka ClosedXML.



\## Import zamówień z Excela



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



\## Załączniki towarów



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



\## Ustawienia portalu



Wygląd portalu publicznego jest sterowany ustawieniami zapisanymi w bazie danych.



Przykładowe ustawienia:



\* kolor główny,

\* kolor tła,

\* kolor tekstu,

\* tytuł stopki,

\* tekst stopki,

\* widoczność elementów portalu.



\## Migracje



Migracje znajdują się w projekcie `Firma.Intranet`.



Najczęstsze polecenia:



```powershell

dotnet ef migrations add NazwaMigracji --project Firma.Intranet

dotnet ef database update --project Firma.Intranet

```



\## Budowanie całego rozwiązania



```powershell

dotnet restore

dotnet build

```



Build powinien zakończyć się bez błędów.



\## Licencja



Projekt zawiera plik `LICENSE`.



