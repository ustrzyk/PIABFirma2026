using Firma.Interfaces.CMS;
using Firma.Interfaces.Sklep;
using Firma.Services.CMS;
using Firma.Services.Sklep;

namespace Firma.PortalWWW
{
    public static class DependencyInjectionFactory
    {
        public static void Resolve(IServiceCollection services, IConfiguration configuration)
        {
            // Rejestruję serwisy CMS
            services.AddScoped<IStronaService, StronaService>();
            services.AddScoped<IAktualnoscService, AktualnoscService>();
            services.AddScoped<IPromocjaService, PromocjaService>();
            services.AddScoped<IUstawieniePortaluService, UstawieniePortaluService>();

            // Rejestruję serwisy sklepu
            services.AddScoped<IRodzajService, RodzajService>();
            services.AddScoped<ITowarService, TowarService>();
            services.AddScoped<IProducentService, ProducentService>();
            services.AddScoped<IStanMagazynowyService, StanMagazynowyService>();
            services.AddScoped<IZamowieniePubliczneService, ZamowieniePubliczneService>();
            services.AddScoped<IStatusZamowieniaService, StatusZamowieniaService>();
        }
    }
}