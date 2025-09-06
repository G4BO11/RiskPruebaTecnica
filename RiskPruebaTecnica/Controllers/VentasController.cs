using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

public class VentasController : Controller
{
    private readonly IVentaService _ventaService;
    private readonly IClienteService _clienteService;
    private readonly IProductoService _productoService;
    private readonly UserManager<IdentityUser> _userManager;

    public VentasController(
        IVentaService ventaService,
        IClienteService clienteService,
        IProductoService productoService,
        UserManager<IdentityUser> userManager)
    {
        _ventaService = ventaService;
        _clienteService = clienteService;
        _productoService = productoService;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var ventas = await _ventaService.GetAllAsync();
        return View(ventas);
    }

    public async Task<IActionResult> Create()
    {
        var productos = await _productoService.GetAllAsync();
        ViewBag.Productos = productos.Where(p => p.UnidadesExistentes > 0).ToList();
        return View(new VentaRequest());
    }

    [HttpPost]
    public async Task<IActionResult> CreateJson([FromBody] VentaRequest ventaRequest)
    {
        try
        {
            // El código que ya tienes...
            var usuario = await _userManager.FindByEmailAsync("admin@supermercado.com");
            if (usuario == null)
                return Json(new { success = false, error = "Usuario no encontrado" });

            var cliente = await _clienteService.GetByIdAsync(ventaRequest.ClienteId);
            if (cliente == null)
                return Json(new { success = false, error = "Cliente no encontrado" });

            var ventaCreada = await _ventaService.CrearVentaAsync(usuario.Id, cliente.Id, ventaRequest.Detalles);

            return Json(new
            {
                success = true,
                ventaId = ventaCreada.Id,
                total = ventaCreada.Total,
                message = $"Venta creada exitosamente. Total: ${ventaCreada.Total:N0}"
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var venta = await _ventaService.GetByIdAsync(id);
        if (venta == null)
            return NotFound();

        return View(venta);
    }

    [HttpGet]
    public async Task<JsonResult> BuscarCliente(string numeroIdentificacion)
    {
        if (string.IsNullOrEmpty(numeroIdentificacion))
            return Json(new { found = false });

        var cliente = await _clienteService.GetByNumeroIdentificacionAsync(numeroIdentificacion);

        if (cliente == null)
        {
            return Json(new
            {
                found = false,
                message = "Cliente no encontrado. ¿Desea registrarlo?"
            });
        }

        return Json(new
        {
            found = true,
            cliente = new
            {
                id = cliente.Id,
                nombre = cliente.Nombre,
                apellido = cliente.Apellido,
                nombreCompleto = $"{cliente.Nombre} {cliente.Apellido}"
            }
        });
    }

    // AJAX: Obtener información del producto
    [HttpGet]
    public async Task<JsonResult> ObtenerProducto(Guid productoId)
    {
        var producto = await _productoService.GetByIdAsync(productoId);
        if (producto == null)
            return Json(new { found = false });

        return Json(new
        {
            found = true,
            producto = new
            {
                id = producto.Id,
                nombre = producto.Nombre,
                precio = producto.ValorUnitario,
                stock = producto.UnidadesExistentes
            }
        });
    }

    // AJAX: Validar stock antes de agregar producto
    [HttpGet]
    public async Task<JsonResult> ValidarStock(Guid productoId, int cantidad)
    {
        var tieneStock = await _productoService.ValidarStockAsync(productoId, cantidad);
        var producto = await _productoService.GetByIdAsync(productoId);

        return Json(new
        {
            tieneStock,
            stockDisponible = producto?.UnidadesExistentes ?? 0
        });
    }

    // POST: Crear cliente rápido desde venta
    [HttpPost]
    public async Task<JsonResult> CrearClienteRapido([FromBody] ClienteDto cliente)
    {
        try
        {
            var clienteCreado = await _clienteService.CreateAsync(cliente);
            return Json(new
            {
                success = true,
                cliente = new
                {
                    id = clienteCreado.Id,
                    nombre = clienteCreado.Nombre,
                    apellido = clienteCreado.Apellido,
                    nombreCompleto = $"{clienteCreado.Nombre} {clienteCreado.Apellido}"
                }
            });
        }
        catch (Exception ex)
        {
            return Json(new { success = false, error = ex.Message });
        }
    }

    private async Task CargarDatosVenta()
    {
        var productos = await _productoService.GetAllAsync();
        ViewBag.Productos = productos.Where(p => p.UnidadesExistentes > 0).ToList();
    }
}