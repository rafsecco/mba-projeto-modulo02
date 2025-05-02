using Api.Extensions;
using Core.Domain.Entities;
using Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromForm] Product product, CancellationToken cancellationToken)
    {
        var id = User.GetUserId();
        product.SellerId = id;

        return await _service.CreateAsync(product, cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> Update([Bind("Name, Description, Price, StockQuantity, CategoryId, SellerId")] Product product, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _service.UpdateAsync(product, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> Get(CancellationToken cancellationToken)
    {
        return await _service.GetAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Product>> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(id, cancellationToken);
    }

    [HttpGet("{categoryId}/categoryId")]
    [AllowAnonymous]
    public async Task<ActionResult<List<Product>>> GetByCategoryId(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _service.GetByCategoryIdAsync(categoryId, cancellationToken);
    }
}