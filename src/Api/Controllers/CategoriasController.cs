using Core.Domain.Entities;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _service;

    public CategoriasController(ICategoriaService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync(CreateCategoriaViewModel createCategoriaViewModel,
        CancellationToken cancellationToken)
    {
        var id = await _service.CreateAsync(createCategoriaViewModel, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, id);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync(UpdateCategoriaViewModel updateCategoriaViewModel,
        CancellationToken cancellationToken)
    {
        try
        {
            await _service.UpdateAsync(updateCategoriaViewModel, cancellationToken);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
    }

    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var succeed = await _service.DeleteAsync(id, cancellationToken);
        if (succeed) return NoContent();
        return BadRequest("Não é possível excluir a categoria, pois existem produtos vinculados a ela.");
    }

    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(List<Categoria>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Categoria>>> GetAsync(CancellationToken cancellationToken)
    {
        var categorias = await _service.GetAsync(cancellationToken);
        return Ok(categorias);
    }

    [AllowAnonymous]
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Categoria), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Categoria>> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        var categoria = await _service.FindAsync(id, cancellationToken);
        if (categoria == null) return NotFound();
        return Ok(categoria);
    }
}