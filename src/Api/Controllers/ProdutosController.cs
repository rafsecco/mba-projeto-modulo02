using Core.Domain.Entities;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Guid>> Create([FromForm] CreateProdutoViewModel createProdutoViewModel,
        CancellationToken cancellationToken)
    {
        try
        {
            var produtoId = await _service.CreateAsync(createProdutoViewModel, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, produtoId);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromBody] UpdateProductViewModel updateProductViewModel,
        CancellationToken cancellationToken)
    {
        try
        {
            await _service.UpdateAsync(updateProductViewModel, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Remove(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await _service.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<Produto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Produto>>> Get(CancellationToken cancellationToken)
    {
        var result = await _service.GetAsync(cancellationToken);
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Produto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Produto>> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        var produto = await _service.FindAsync(id, cancellationToken);

        if (produto == null)
            return NotFound();

        return Ok(produto);
    }

    [AllowAnonymous]
    [HttpGet("{categoriaId}/categoriaId")]
    [ProducesResponseType(typeof(List<Produto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Produto>>> GetByCategoryId(Guid categoriaId, CancellationToken cancellationToken)
    {
        var produtos = await _service.GetByCategoriaIdAsync(categoriaId, cancellationToken);

        return Ok(produtos);
    }
}