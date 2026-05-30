using System.Globalization;
using Firma.Data.Data;
using Firma.Intranet.ModelBinders;
using Microsoft.EntityFrameworkCore;

var cultureInfo = new CultureInfo("pl-PL");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("FirmaContext")
    ?? throw new InvalidOperationException("Connection string 'FirmaContext' not found.");

builder.Services.AddDbContext<FirmaContext>(options => options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    // Pozwala wpisywać kwoty z przecinkiem i kropką, np. 12,50 albo 12.50.
    options.ModelBinderProviders.Insert(0, new DecimalModelBinderProvider());
});

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
