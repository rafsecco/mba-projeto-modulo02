//using Core.Extensions;

using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace AppMvc.Controllers;

public class ProductsController : Controller
{
    private readonly ICategoryService _categoryService;
    private readonly IProductService _productService;

    public ProductsController(IProductService productService, ICategoryService categoryService)
    {
        _productService = productService;
        _categoryService = categoryService;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var products = await _productService.GetAsync(cancellationToken);
        return View(products);
    }

    public async Task<IActionResult> Details(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        if (product is null) return NotFound();

        return View(product);
    }

    public async Task<IActionResult> Create(CancellationToken cancellationToken)
    {
        var categories = await _categoryService.GetAsync(cancellationToken);
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");
        //ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId");
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(
        CreateProductViewModel createProductViewModel, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
            await _productService.CreateAsync(createProductViewModel, Guid.Parse(userId), cancellationToken);
            return RedirectToAction(nameof(Index));
        }

        var categories = await _categoryService.GetAsync(cancellationToken);
        ViewData["CategoryId"] = new SelectList(categories, "Id", "Name");

        return View(createProductViewModel);
    }

    public async Task<IActionResult> Edit(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        if (product == null) return NotFound();
        //var categories = await _categoryService.GetAsync(cancellationToken);
        //ViewData["CategoryId"] = new SelectList(categories, "Id", "Name", product.CategoryId);
        //ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId", product.SellerId);

        var productViewModel = new UpdateProductViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock
        };
        return View(productViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id,
        UpdateProductViewModel updateProductViewModel, CancellationToken cancellationToken)
    {
        if (id != updateProductViewModel.Id) return NotFound();

        if (ModelState.IsValid)
        {
            await _productService.UpdateAsync(updateProductViewModel, cancellationToken);

            return RedirectToAction(nameof(Index));
        }

        //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Description", product.CategoryId);
        //ViewData["SellerId"] = new SelectList(_context.Sellers, "UserId", "UserId", product.SellerId);
        return View(updateProductViewModel);
    }

    public async Task<IActionResult> Delete(Guid? id, CancellationToken cancellationToken)
    {
        if (id == null) return NotFound();

        var product = await _productService.GetByIdAsync(id.Value, cancellationToken);
        if (product == null) return NotFound();

        return View(product);
    }

    [HttpPost]
    [ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id, CancellationToken cancellationToken)
    {
        var product = await _productService.GetByIdAsync(id, cancellationToken);
        if (product != null) await _productService.DeleteAsync(id, cancellationToken);

        return RedirectToAction(nameof(Index));
    }
}