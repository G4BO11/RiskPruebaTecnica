using Microsoft.AspNetCore.Identity;
using RiskPruebaTecnica.Models.Entities;

namespace RiskPruebaTecnica.Data;

public static class SeedData
{
    public static async void Initialize(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        await context.Database.EnsureCreatedAsync();

        var adminUser = await userManager.FindByEmailAsync("admin@supermercado.com");
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = "admin@supermercado.com",
                Email = "admin@supermercado.com",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin123!");
            if (!result.Succeeded)
            {
                throw new Exception($"Error creando usuario admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        // Productos
        if (!context.Productos.Any())
        {
            var productos = new List<Producto>
            {
                new Producto { Codigo = "P001", Nombre = "Arroz Diana 1kg", ValorUnitario = 3500, UnidadesExistentes = 100, FechaCreacion = DateTime.UtcNow, Activo = true },
                new Producto { Codigo = "P002", Nombre = "Leche Alpina 1L", ValorUnitario = 4200, UnidadesExistentes = 50, FechaCreacion = DateTime.UtcNow, Activo = true },
                new Producto { Codigo = "P003", Nombre = "Pan Tajado Bimbo", ValorUnitario = 2800, UnidadesExistentes = 30, FechaCreacion = DateTime.UtcNow, Activo = true }
            };

            context.Productos.AddRange(productos);
            await context.SaveChangesAsync();
        }
    }
}