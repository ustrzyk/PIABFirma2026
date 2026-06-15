using Firma.Data.Data;
using Firma.Data.Data.Sklep;
using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Services.Data.Intranet;
using Microsoft.EntityFrameworkCore;

namespace Firma.Intranet.Services.Intranet
{
    public class ZalacznikTowaruIntranetService : IZalacznikTowaruIntranetService
    {
        private readonly FirmaContext _context;

        public ZalacznikTowaruIntranetService(FirmaContext context)
        {
            _context = context;
        }

        public async Task<List<ZalacznikTowaru>> PobierzListe()
        {
            return await _context.ZalacznikTowaru
                .Include(z => z.Towar)
                .OrderByDescending(z => z.DataDodania)
                .ToListAsync();
        }

        public async Task<ZalacznikTowaru?> PobierzSzczegoly(int id)
        {
            return await _context.ZalacznikTowaru
                .Include(z => z.Towar)
                .FirstOrDefaultAsync(z => z.IdZalacznikaTowaru == id);
        }

        public async Task<ZalacznikTowaru?> PobierzDoEdycji(int id)
        {
            return await _context.ZalacznikTowaru
                .FirstOrDefaultAsync(z => z.IdZalacznikaTowaru == id);
        }

        public async Task<ZalacznikTowaru?> PobierzDoUsuniecia(int id)
        {
            return await _context.ZalacznikTowaru
                .Include(z => z.Towar)
                .FirstOrDefaultAsync(z => z.IdZalacznikaTowaru == id);
        }

        public async Task<ZalacznikTowaru?> PobierzDoPobrania(int id)
        {
            return await _context.ZalacznikTowaru
                .FirstOrDefaultAsync(z => z.IdZalacznikaTowaru == id);
        }

        public async Task Dodaj(int idTowaru, string opis, PlikZalacznikaDto plik, string folderUploadu)
        {
            var zapisanyPlik = await ZapiszPlik(plik, folderUploadu);

            var zalacznik = new ZalacznikTowaru
            {
                IdTowaru = idTowaru,
                NazwaPliku = zapisanyPlik.NazwaPliku,
                NazwaOryginalna = Path.GetFileName(plik.NazwaOryginalna),
                Sciezka = zapisanyPlik.Sciezka,
                TypPliku = plik.ContentType,
                Rozmiar = plik.Rozmiar,
                Opis = opis,
                DataDodania = DateTime.Now,
                CzyAktywny = true
            };

            _context.ZalacznikTowaru.Add(zalacznik);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> Aktualizuj(int id, int idTowaru, string opis, PlikZalacznikaDto? plik, string folderUploadu)
        {
            var zalacznik = await _context.ZalacznikTowaru
                .FirstOrDefaultAsync(z => z.IdZalacznikaTowaru == id);

            if (zalacznik == null)
            {
                return false;
            }

            zalacznik.IdTowaru = idTowaru;
            zalacznik.Opis = opis;

            if (plik != null)
            {
                UsunPlik(folderUploadu, zalacznik.Sciezka);

                var zapisanyPlik = await ZapiszPlik(plik, folderUploadu);

                zalacznik.NazwaPliku = zapisanyPlik.NazwaPliku;
                zalacznik.NazwaOryginalna = Path.GetFileName(plik.NazwaOryginalna);
                zalacznik.Sciezka = zapisanyPlik.Sciezka;
                zalacznik.TypPliku = plik.ContentType;
                zalacznik.Rozmiar = plik.Rozmiar;
                zalacznik.DataDodania = DateTime.Now;
            }

            _context.ZalacznikTowaru.Update(zalacznik);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task Usun(int id, string folderUploadu)
        {
            var zalacznik = await _context.ZalacznikTowaru
                .FirstOrDefaultAsync(z => z.IdZalacznikaTowaru == id);

            if (zalacznik != null)
            {
                UsunPlik(folderUploadu, zalacznik.Sciezka);

                _context.ZalacznikTowaru.Remove(zalacznik);

                await _context.SaveChangesAsync();
            }
        }

        public async Task UsunZaznaczone(int[] ids, string folderUploadu)
        {
            if (ids == null || ids.Length == 0)
            {
                return;
            }

            var zalaczniki = await _context.ZalacznikTowaru
                .Where(z => ids.Contains(z.IdZalacznikaTowaru))
                .ToListAsync();

            foreach (var zalacznik in zalaczniki)
            {
                UsunPlik(folderUploadu, zalacznik.Sciezka);
            }

            _context.ZalacznikTowaru.RemoveRange(zalaczniki);

            await _context.SaveChangesAsync();
        }

        public async Task<List<TowarSelectItemDto>> PobierzTowaryDoSelectList()
        {
            return await _context.Towar
                .OrderBy(t => t.Nazwa)
                .Select(t => new TowarSelectItemDto
                {
                    IdTowaru = t.IdTowaru,
                    Nazwa = t.Nazwa
                })
                .ToListAsync();
        }

        public string PobierzSciezkeFizyczna(string folderUploadu, string sciezka)
        {
            var nazwaPliku = sciezka
                .Split('/', StringSplitOptions.RemoveEmptyEntries)
                .LastOrDefault() ?? string.Empty;

            return Path.Combine(folderUploadu, nazwaPliku);
        }

        private async Task<(string NazwaPliku, string Sciezka)> ZapiszPlik(
            PlikZalacznikaDto plik,
            string folderUploadu)
        {
            Directory.CreateDirectory(folderUploadu);

            var rozszerzenie = Path.GetExtension(plik.NazwaOryginalna).ToLowerInvariant();
            var nazwaPliku = $"{Guid.NewGuid():N}{rozszerzenie}";
            var sciezkaFizyczna = Path.Combine(folderUploadu, nazwaPliku);

            using (var fileStream = new FileStream(sciezkaFizyczna, FileMode.Create))
            {
                await plik.Stream.CopyToAsync(fileStream);
            }

            var sciezkaPubliczna = $"/uploads/towary/{nazwaPliku}";

            return (nazwaPliku, sciezkaPubliczna);
        }

        private void UsunPlik(string folderUploadu, string sciezka)
        {
            var sciezkaFizyczna = PobierzSciezkeFizyczna(folderUploadu, sciezka);

            if (File.Exists(sciezkaFizyczna))
            {
                File.Delete(sciezkaFizyczna);
            }
        }
    }
}