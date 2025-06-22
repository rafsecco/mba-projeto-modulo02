using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Controllers;

[Authorize(Roles = "Admin")]
[Route("vendedores")]
public class VendedoresController : Controller
{
	private readonly ISellerService _sellerService;

	public VendedoresController(ISellerService sellerService)
	{
		_sellerService = sellerService;
	}

	public async Task<IActionResult> Index(CancellationToken cancellationToken)
	{
		return View(await _sellerService.GetAsync(cancellationToken));
	}
}
