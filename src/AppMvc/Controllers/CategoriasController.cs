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

        var categoria = await _categoriaService.FindAsync(id.Value, cancellationToken);

        if (categoria == null) return NotFound();

        return View(categoria);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoriaViewModel createCategoriaViewModel,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            await _categoriaService.CreateAsync(createCategoriaViewModel, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        return View(createCategoriaViewModel);
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var categoria = await _categoriaService.FindAsync(id.Value, cancellationToken);
        if (categoria == null) return NotFound();

        var updateCategoryViewModel = new UpdateCategoriaViewModel
        {
            Id = categoria.Id,
            Nome = categoria.Nome,
            Descricao = categoria.Descricao
        };
        return View(updateCategoryViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateCategoriaViewModel updateCategoriaViewModel,
        CancellationToken cancellationToken)
    {
        if (id != updateCategoriaViewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _categoriaService.UpdateAsync(updateCategoriaViewModel, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        return View(updateCategoriaViewModel);
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var categoria = await _categoriaService.FindAsync(id.Value, cancellationToken);
        if (categoria == null) return NotFound();

        return View(categoria);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var result = await _categoriaService.DeleteAsync(id, cancellationToken);
        if (!result)
        {
            TempData["Error"] = "Não é possível excluir a categoria, pois existem produtos vinculados a ela.";
            return RedirectToAction(nameof(Delete), new { id });
        }
        return RedirectToAction(nameof(Index));
    }
}