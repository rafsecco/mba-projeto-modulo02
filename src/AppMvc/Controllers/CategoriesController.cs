using Core.Data;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppMvc.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ApplicationDbContext context, ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [AllowAnonymous]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        return View(await _categoryService.GetAsync(cancellationToken));
    }

    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var category = await _categoryService.FindAsync(id.Value, cancellationToken);

        if (category == null) return NotFound();

        return View(category);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryViewModel createCategoryViewModel,
        CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            await _categoryService.CreateAsync(createCategoryViewModel, cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        return View(createCategoryViewModel);
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var category = await _categoryService.FindAsync(id.Value, cancellationToken);
        if (category == null) return NotFound();

        var updateCategoryViewModel = new UpdateCategoryViewModel
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        return View(updateCategoryViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, UpdateCategoryViewModel updateCategoryViewModel,
        CancellationToken cancellationToken)
    {
        if (id != updateCategoryViewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _categoryService.UpdateAsync(updateCategoryViewModel, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        return View(updateCategoryViewModel);
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var category = await _categoryService.FindAsync(id.Value, cancellationToken);
        if (category == null) return NotFound();

        return View(category);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var category = await _categoryService.FindAsync(id, cancellationToken);
       var result= await _categoryService.DeleteAsync(id, cancellationToken);
       if (!result)
       {
           TempData["Error"] = "Não é possível excluir a categoria, pois existem produtos vinculados a ela.";
           return RedirectToAction(nameof(Delete), new { id });
       }
        return RedirectToAction(nameof(Index));
    }
}