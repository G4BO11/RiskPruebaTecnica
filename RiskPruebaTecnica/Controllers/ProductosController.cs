using Microsoft.AspNetCore.Mvc;

public class ProductosController : Controller
{
    private readonly IProductoService _productoService;

    public ProductosController(IProductoService productoService)
    {
        _productoService = productoService;
    }

    public async Task<IActionResult> Index()
    {
        var productos = await _productoService.GetAllAsync();
        return View(productos);
    }

    public IActionResult Create()
    {
        return View(new ProductoDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductoDto producto)
    {
        if (!ModelState.IsValid)
            return View(producto);

        try
        {
            await _productoService.CreateAsync(producto);
            TempData["Success"] = "Producto creado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(producto);
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var producto = await _productoService.GetByIdAsync(id);
        if (producto == null)
            return NotFound();

        return View(producto);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductoDto producto)
    {
        if (!ModelState.IsValid)
            return View(producto);

        try
        {
            await _productoService.UpdateAsync(producto);
            TempData["Success"] = "Producto actualizado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(producto);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _productoService.DeleteAsync(id);
            TempData["Success"] = "Producto eliminado exitosamente";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }
}