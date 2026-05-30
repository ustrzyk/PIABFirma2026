using System.Globalization;
using Firma.Data.Data;
using Firma.PortalWWW;
using Microsoft.EntityFrameworkCore;

var cultureInfo = new CultureInfo("pl-PL");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("FirmaContext")
    ?? throw new InvalidOperationException("Connection string 'FirmaContext' not found.");

builder.Services.AddDbContext<FirmaContext>(options =>
    options.UseSqlServer(connectionString));

// Rejestracja własnych serwisów 
DependencyInjectionFactory.Resolve(builder.Services, builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();