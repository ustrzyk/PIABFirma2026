using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Firma.Intranet.Services.Intranet
{
    public static class UzytkownikStartowySeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            await DodajRole(roleManager);

            await DodajUzytkownika(
                userManager,
                "tsaran@test.pl",
                "Tsaran123!",
                "Administrator");
        }

        private static async Task DodajRole(RoleManager<IdentityRole> roleManager)
        {
            var role = new[]
            {
                "Administrator",
                "Pracownik"
            };

            foreach (var nazwaRoli in role)
            {
                if (!await roleManager.RoleExistsAsync(nazwaRoli))
                {
                    await roleManager.CreateAsync(new IdentityRole(nazwaRoli));
                }
            }
        }

        private static async Task DodajUzytkownika(
            UserManager<IdentityUser> userManager,
            string email,
            string haslo,
            string rola)
        {
            var uzytkownik = await userManager.FindByEmailAsync(email);

            if (uzytkownik == null)
            {
                uzytkownik = new IdentityUser
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(uzytkownik, haslo);
            }

            if (!await userManager.IsInRoleAsync(uzytkownik, rola))
            {
                await userManager.AddToRoleAsync(uzytkownik, rola);
            }
        }
    }
}