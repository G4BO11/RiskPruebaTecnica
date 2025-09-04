using RiskPruebaTecnica.Models.Entities;

namespace RiskPruebaTecnica.Data;

public static class SeedData
{
    public static void Initialize(ApplicationDbContext context)
    {
        if (context.Usuarios.Any()) return;
        
        var usuario = new Usuario
        {
            NombreUsuario = "admin", 
            Email = "admin@supermercado.com",
            PasswordHash = "admin123",
            FechaCreacion = DateTime.Now,
            Activo = true
        };
        
        var productos = new List<Producto>
        {
            new Producto { Codigo = "P001", Nombre = "Arroz Diana 1kg", ValorUnitario = 3500, UnidadesExistentes = 100, FechaCreacion = DateTime.Now, Activo = true },
            new Producto { Codigo = "P002", Nombre = "Leche Alpina 1L", ValorUnitario = 4200, UnidadesExistentes = 50, FechaCreacion = DateTime.Now, Activo = true },
            new Producto { Codigo = "P003", Nombre = "Pan Tajado Bimbo", ValorUnitario = 2800, UnidadesExistentes = 30, FechaCreacion = DateTime.Now, Activo = true }
        };
        
        context.Usuarios.Add(usuario);
        context.Productos.AddRange(productos);
        context.SaveChanges();
    }
}