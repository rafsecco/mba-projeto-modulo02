using Core.Domain.Entities;
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

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {

        List<Produto> produtos;

        if (User.IsInRole("Admin"))
        {
            produtos = await _produtoService.GetAllAsync(cancellationToken);
        }
        else
        {
            produtos = await _produtoService.GetByVendedorId(cancellationToken);
        }   

        
        return View(produtos);
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var produto = await _produtoService.FindAsync(id.Value, cancellationToken);
        if (produto is null) return NotFound();

        return View(produto);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var categorias = await _categoriaService.GetAsync(cancellationToken);
        ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Nome");
        //ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CriaProdutoViewModel criaProdutoViewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _produtoService.CreateAsync(criaProdutoViewModel, cancellationToken, "wwwroot");
            return RedirectToAction(nameof(Index));
        }

        var categorias = await _categoriaService.GetAsync(cancellationToken);
        ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Nome");

        return View(criaProdutoViewModel);
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var produto = await _produtoService.FindAsync(id.Value, cancellationToken);
        if (produto == null) return NotFound();

        var updateProdutoViewModel = new AtualizaProdutoViewModel
        {
            Id = produto.Id,
            Nome = produto.Nome,
            Descricao = produto.Descricao,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            Ativo = produto.Ativo
        };
        return View(updateProdutoViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        AtualizaProdutoViewModel atualizaProdutoViewModel, CancellationToken cancellationToken)
    {
        if (id != atualizaProdutoViewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _produtoService.UpdateAsync(atualizaProdutoViewModel, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
        //ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId", product.SellerId);
        return View(atualizaProdutoViewModel);
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

    [HttpPost]
    public async Task<IActionResult> ToggleStatus(Guid id, bool ativo, CancellationToken cancellationToken)
    {
        var produto = await _produtoService.FindAsync(id, cancellationToken);
        if (produto == null) return NotFound();

        var atualizaProdutoViewModel = new AtualizaProdutoViewModel
        {
            Id = produto.Id,
            Ativo = ativo
        };
        await _produtoService.UpdateAsync(atualizaProdutoViewModel, cancellationToken); // ou o método que você usa

        return RedirectToAction(nameof(Index));
    }
}