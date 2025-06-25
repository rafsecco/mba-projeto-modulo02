using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("vendedores")]
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
}
