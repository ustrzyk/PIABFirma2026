using Firma.Intranet.Interfaces.Intranet;
using Firma.Intranet.Services;
using Firma.Intranet.Services.Intranet;

namespace Firma.Intranet
{
    public static class DependencyInjectionFactory
    {
        public static void Resolve(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IZamowienieIntranetService, ZamowienieIntranetService>();
            services.AddScoped<ITowarIntranetService, TowarIntranetService>();

            services.AddScoped<FakturaPdfGenerator>();
            services.AddScoped<ZamowienieExcelGenerator>();
            services.AddScoped<ZamowienieExcelSzablonGenerator>();
            services.AddScoped<ZamowienieExcelImporter>();
        }
    }
}