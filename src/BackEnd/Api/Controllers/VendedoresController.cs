using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class VendedoresController : ControllerBase
{
    private readonly IVendedorService _vendedorService;

    public VendedoresController(IVendedorService vendedorService)
    {
        _vendedorService = vendedorService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<Vendedor>>> Get(CancellationToken cancellationToken)
    {
        var resultado = await _vendedorService.GetAsync(cancellationToken);

        if (resultado == null)
            return NotFound();

        return Ok(resultado);
    }

    [HttpPut("{id}/Ativo")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> AtualizaAtivo(Guid id, bool ativo, CancellationToken cancellationToken)
    {
        await _vendedorService.AtualizaAtivoAsync(id, ativo, cancellationToken);
        return NoContent();
    }
}