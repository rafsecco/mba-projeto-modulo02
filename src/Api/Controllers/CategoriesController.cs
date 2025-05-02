using Core.Data.Repositories;
using Core.Domain.Entities;
using Core.Services;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync([Bind("Name,Description")] Category category, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _service.CreateAsync(category, cancellationToken);

        return Ok(category);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync([Bind("Name,Description")] Category category, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _service.UpdateAsync(category, cancellationToken);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var succeed = await _service.DeleteAsync(id, cancellationToken);
        if (succeed) return NoContent();
        return BadRequest("Não é possível excluir a categoria, pois existem produtos vinculados a ela.");
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<List<Category>>> GetAsync(CancellationToken cancellationToken)
    {
        return await _service.GetAsync(cancellationToken);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<ActionResult<Category>> FindAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _service.GetByIdAsync(id, cancellationToken);
    }
}