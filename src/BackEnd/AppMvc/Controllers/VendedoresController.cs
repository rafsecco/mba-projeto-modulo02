using Business.Interfaces;
using Business.Services;
using Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("[controller]")]
public class VendedoresController : Controller
{
	private readonly IVendedorService _vendedorService;

	public VendedoresController(IVendedorService vendedorService)
	{
		_vendedorService = vendedorService;
	}

	public async Task<IActionResult> Index(CancellationToken cancellationToken)
	{
		return View(await _vendedorService.GetAsync(cancellationToken));
	}

    [HttpPost]
    public async Task<IActionResult> AtualizaAtivo(Guid id, bool ativo, CancellationToken cancellationToken)
    {
        
        await _vendedorService.AtualizaAtivoAsync(id,ativo, cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}
