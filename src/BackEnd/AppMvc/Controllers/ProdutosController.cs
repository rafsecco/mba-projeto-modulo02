using Business.Extensions;
using Business.Interfaces;
using Business.Models;
using Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace AppMvc.Controllers;

[Authorize(Roles = "Admin,Vendedor")]
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
            produtos = await _produtoService.GetAsync(cancellationToken);
        }
        else
        {
            produtos = await _produtoService.GetByVendedorId(cancellationToken);
        }

        return View(produtos);
    }

    [ClaimsAuthorize("Produtos", "VI")]
    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var produto = await _produtoService.FindAsync(id.Value, cancellationToken);
        if (produto is null) return NotFound();

        return View(produto);
    }

    [ClaimsAuthorize("Produtos", "AD")]
    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var categorias = await _categoriaService.GetAsync(cancellationToken);
        ViewData["CategoriaId"] = new SelectList(categorias, "Id", "Nome");
        //ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId");
        return View();
    }

    [HttpPost]
    [ClaimsAuthorize("Produtos", "AD")]
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

    [ClaimsAuthorize("Produtos", "ED")]
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
			Imagem = produto.Imagem,
            Preco = produto.Preco,
            Estoque = produto.Estoque,
            Ativo = produto.Ativo
        };
        return View(updateProdutoViewModel);
    }

    [HttpPost]
    [ClaimsAuthorize("Produtos", "ED")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        AtualizaProdutoViewModel atualizaProdutoViewModel, CancellationToken cancellationToken)
    {
        if (id != atualizaProdutoViewModel.Id) return NotFound();

		if (!ModelState.IsValid)
        {
			return View(atualizaProdutoViewModel);
        }

		var produtoDb = await _produtoService.FindAsync(id, cancellationToken);

		atualizaProdutoViewModel.Imagem = produtoDb.Imagem;

		if (atualizaProdutoViewModel.ImagemUpload != null)
		{
			atualizaProdutoViewModel.Imagem = atualizaProdutoViewModel.ImagemUpload.FileName;
		}

		await _produtoService.UpdateAsync(atualizaProdutoViewModel, cancellationToken, "wwwroot");

		return RedirectToAction(nameof(Index));

		//ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
		//ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId", product.SellerId);

	}

    [ClaimsAuthorize("Produtos", "EX")]
    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var produto = await _produtoService.FindAsync(id.Value, cancellationToken);
        if (produto == null) return NotFound();

        return View(produto);
    }

    [HttpPost]
    [ClaimsAuthorize("Produtos", "EX")]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _produtoService.FindAsync(id, cancellationToken);
        if (produto != null) await _produtoService.DeleteAsync(id, cancellationToken);

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> AtualizaAtivo(Guid id, bool ativo, CancellationToken cancellationToken)
    {
        var atualizaProdutoViewModel = new AtualizaProdutoViewModel
        {
            Id = id,
            Ativo = ativo
        };

        await _produtoService.UpdateAsync(atualizaProdutoViewModel, cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}
