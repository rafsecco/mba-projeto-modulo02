using Core.Data;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Controllers;

[Authorize]
public class CategoriasController : Controller
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ApplicationDbContext context, ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await _categoriaService.GetAsync(cancellationToken));
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var category = await _categoriaService.FindAsync(id.Value, cancellationToken);

        if (category == null) return NotFound();

        return View(category);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CriaCategoriaViewModel criaCategoriaViewModel,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            await _categoriaService.CreateAsync(criaCategoriaViewModel, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        return View(criaCategoriaViewModel);
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var category = await _categoriaService.FindAsync(id.Value, cancellationToken);
        if (category == null) return NotFound();

        var updateCategoryViewModel = new AtualizaCategoriaViewModel
        {
            Id = category.Id,
            Name = category.Nome,
            Description = category.Descricao
        };
        return View(updateCategoryViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, AtualizaCategoriaViewModel atualizaCategoriaViewModel,
        CancellationToken cancellationToken)
    {
        if (id != atualizaCategoriaViewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _categoriaService.UpdateAsync(atualizaCategoriaViewModel, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        return View(atualizaCategoriaViewModel);
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var category = await _categoriaService.FindAsync(id.Value, cancellationToken);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoriaService.FindAsync(id, cancellationToken);
        var result = await _categoriaService.DeleteAsync(id, cancellationToken);
        if (!result)
        {
            TempData["Error"] = "Não é possível excluir a categoria, pois existem produtos vinculados a ela.";
            return RedirectToAction(nameof(Delete), new { id });
        }
        return RedirectToAction(nameof(Index));
    }
}