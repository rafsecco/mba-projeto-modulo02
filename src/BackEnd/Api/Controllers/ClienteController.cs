using Business.Interfaces;
using Business.Models;
using Business.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin,Cliente")]
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

    [HttpPost("{produtoId}/Favoritos")]
    [ProducesResponseType(typeof(Favorito), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddFavorito(Guid produtoId, CancellationToken cancellationToken)
    {
        var favorito = await _clienteService.AddFavoritoAsync(produtoId, cancellationToken);
        if (favorito is not null)
            return StatusCode(StatusCodes.Status201Created, favorito);

        return Problem("Erro ao adicionar favorito");
    }

    [HttpDelete("{produtoId}/Favoritos")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveFavorito(Guid produtoId, CancellationToken cancellationToken)
    {
        await _clienteService.RemoveFavoritoAsync(produtoId, cancellationToken);
        return NoContent();
    }

    [HttpGet("Favoritos")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<List<Favorito>>> GetFavoritos(CancellationToken cancellationToken)
    {
        var favoritos = await _clienteService.GetFavoritosAsync(cancellationToken);
        return Ok(favoritos);
    }
}