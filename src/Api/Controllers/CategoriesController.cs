using Core.Domain.Entities;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _service;

    public CategoriesController(ICategoryService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    public async Task<IActionResult> CreateAsync(CreateCategoryViewModel createCategoryViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        var id = await _service.CreateAsync(createCategoryViewModel, cancellationToken);

        return Ok(id);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateAsync(UpdateCategoryViewModel updateCategoryViewModel, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        await _service.UpdateAsync(updateCategoryViewModel, cancellationToken);
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
        return await _service.FindAsync(id, cancellationToken);
    }
}