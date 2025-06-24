using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace AppMvc.Controllers;

[Authorize]
public class ProdutosController : Controller
{
    private readonly ICategoriaService _categoriaService;
    private readonly IProdutoService _produtoService;

    public ProdutosController(IProdutoService produtoService, ICategoriaService categoriaService)
    {
        _produtoService = produtoService;
        _categoriaService = categoriaService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var produtos = await _produtoService.GetByVendedorId(cancellationToken);
        return View(produtos);
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var produto = await _produtoService.FindAsync(id.Value, cancellationToken);
        if (produto is null) return NotFound();

        return View(produto);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var categories = await _categoriaService.GetAsync(cancellationToken);
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
        //ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateProdutoViewModel createProdutoViewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _produtoService.CreateAsync(createProdutoViewModel, cancellationToken, "wwwroot");
            return RedirectToAction(nameof(Index));
        }

        var categories = await _categoriaService.GetAsync(cancellationToken);
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");

        return View(createProdutoViewModel);
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var produto = await _produtoService.FindAsync(id.Value, cancellationToken);
        if (produto == null) return NotFound();

        var productViewModel = new UpdateProductViewModel
        {
            Id = produto.Id,
            Name = produto.Nome,
            Description = produto.Descricao,
            Price = produto.Preco,
            Stock = produto.Estoque
        };
        return View(productViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        UpdateProductViewModel updateProdutoViewModel, CancellationToken cancellationToken)
    {
        if (id != updateProdutoViewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _produtoService.UpdateAsync(updateProdutoViewModel, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
        //ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId", product.SellerId);
        return View(updateProdutoViewModel);
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var produto = await _produtoService.FindAsync(id.Value, cancellationToken);
        if (produto == null) return NotFound();

        return View(produto);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _produtoService.FindAsync(id, cancellationToken);
        if (produto != null) await _produtoService.DeleteAsync(id, cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}