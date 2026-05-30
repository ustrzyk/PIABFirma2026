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
            // Serwisy CMS
            services.AddScoped<IStronaService, StronaService>();
            services.AddScoped<IAktualnoscService, AktualnoscService>();

            // Serwisy sklepu
            services.AddScoped<IRodzajService, RodzajService>();
            services.AddScoped<ITowarService, TowarService>();
        }
    }
}