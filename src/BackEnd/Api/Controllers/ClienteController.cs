using Business.Interfaces;
using Business.Models;
using Business.Services;
using Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserViewModel userViewModel,
        CancellationToken cancellationToken)
    {
        var userId = await _clienteService.CriaAsync(userViewModel, cancellationToken);

        if (userId.HasValue)
            return StatusCode(StatusCodes.Status201Created, userId);

        return Problem("Erro ao registrar cliente");
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<Cliente>>> Get(CancellationToken cancellationToken)
    {
        var resultado = await _clienteService.GetAsync(cancellationToken);

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
        await _clienteService.AtualizaAtivoAsync(id, ativo, cancellationToken);
        return NoContent();
    }

}