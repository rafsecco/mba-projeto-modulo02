using Api.Extensions;
using Core.Domain.Entities;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> Create([FromForm] CreateProductViewModel createProductViewModel,
        CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        return await _service.CreateAsync(createProductViewModel, cancellationToken);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateProductViewModel updateProductViewModel,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _service.UpdateAsync(updateProductViewModel, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Product>>> Get(CancellationToken cancellationToken)
    {
        var result = await _service.GetAsync(cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Product>> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        var product = await _service.FindAsync(id, cancellationToken);

        if (product == null)
            return NotFound();

        return Ok(product);
    }

    [AllowAnonymous]
    [HttpGet("{categoryId}/categoryId")]
    [ProducesResponseType(typeof(List<Product>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Product>>> GetByCategoryId(Guid categoryId, CancellationToken cancellationToken)
    {
        var products = await _service.GetByCategoryIdAsync(categoryId, cancellationToken);

        return Ok(products);
    }
}