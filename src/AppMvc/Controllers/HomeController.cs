using AppMvc.Models;
using Core.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AppMvc.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProdutoService _produtoService;

    public HomeController(ILogger<HomeController> logger, IProdutoService produtoService)
    {
        _logger = logger;
        _produtoService = produtoService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var produtos = await _produtoService.GetByVendedorId(cancellationToken);
        ViewData["IsHome"] = true;
        return View(produtos);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}