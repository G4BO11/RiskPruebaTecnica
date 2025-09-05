using Microsoft.AspNetCore.Mvc;

public class ClientesController : Controller
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    public async Task<IActionResult> Index()
    {
        var clientes = await _clienteService.GetAllAsync();
        return View(clientes);
    }

    public IActionResult Create()
    {
        return View(new ClienteDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create(ClienteDto cliente)
    {
        if (!ModelState.IsValid)
            return View(cliente);

        try
        {
            await _clienteService.CreateAsync(cliente);
            TempData["Success"] = "Cliente creado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(cliente);
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var cliente = await _clienteService.GetByIdAsync(id);
        if (cliente == null)
            return NotFound();

        return View(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(ClienteDto cliente)
    {
        if (!ModelState.IsValid)
            return View(cliente);

        try
        {
            await _clienteService.UpdateAsync(cliente);
            TempData["Success"] = "Cliente actualizado exitosamente";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(cliente);
        }
    }


    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _clienteService.DeleteAsync(id);
            TempData["Success"] = "Cliente eliminado exitosamente";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }

        return RedirectToAction(nameof(Index));
    }

    // AJAX: Buscar cliente por identificación (para el proceso de venta)
    [HttpGet]
    public async Task<JsonResult> BuscarPorIdentificacion(string numeroIdentificacion)
    {
        var cliente = await _clienteService.GetByNumeroIdentificacionAsync(numeroIdentificacion);
        if (cliente == null)
            return Json(new { found = false });

        return Json(new { found = true, cliente });
    }
}