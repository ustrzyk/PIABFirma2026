using System.Globalization;
using Firma.Data.Data;
using Firma.PortalWWW;
using Firma.PortalWWW.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var cultureInfo = new CultureInfo("pl-PL");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("FirmaContext")
    ?? throw new InvalidOperationException("Connection string 'FirmaContext' not found.");

builder.Services.AddDbContext<FirmaContext>(options =>
    options.UseSqlServer(connectionString));

DependencyInjectionFactory.Resolve(builder.Services, builder.Configuration);

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;

        options.User.RequireUniqueEmail = true;
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddErrorDescriber<PolskiIdentityErrorDescriber>()
    .AddEntityFrameworkStores<FirmaContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = ".Firma.PortalWWW.KontoKlienta";
    options.LoginPath = "/KontoKlienta/Logowanie";
    options.AccessDeniedPath = "/KontoKlienta/Logowanie";
});

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Firma.PortalWWW.Koszyk";
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();