using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RiskPruebaTecnica.Models;

namespace RiskPruebaTecnica.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IVentaService _ventaService;
    private readonly IProductoService _productoService;

    public HomeController(ILogger<HomeController> logger, IVentaService ventaService, IProductoService productoService)
    {
        _logger = logger;
        _ventaService = ventaService;
        _productoService = productoService;
    }

    public async Task<IActionResult> Index()
    {
        var ventasHoy = await _ventaService.GetVentasByFechaAsync(DateTime.UtcNow.Date);
        var productos = await _productoService.GetAllAsync();

        var viewModel = new DashboardViewModel
        {
            VentasHoy = ventasHoy.Count(),
            TotalVentasHoy = ventasHoy.Sum(v => v.Total),
            ProductosBajoStock = productos.Count(p => p.UnidadesExistentes < 10),
            UltimasVentas = ventasHoy.OrderByDescending(v => v.FechaVenta).Take(5).ToList()
        };

        return View(viewModel);
    }


}