using Api.Models;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly JwtSettings _jwtSettings;
    private readonly IUserService _userService;

    public UsersController(IOptions<JwtSettings> jwtSettings, IUserService userService)
    {
        _userService = userService;

        _jwtSettings = jwtSettings.Value;
    }

    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserViewModel userViewModel,
        CancellationToken cancellationToken)
    {
        var userId = await _userService.RegisterAsync(userViewModel, cancellationToken);

        if (userId.HasValue)
            return StatusCode(StatusCodes.Status201Created, userId);

        return Problem("Erro ao registrar usuário");
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserViewModel userViewModel, CancellationToken cancellationToken)
    {
        var userId = await _userService.LoginAsync(userViewModel, cancellationToken);
        if (userId.HasValue) return Ok(new { token = GeneratesJwt(userId.Value) });
        return Problem("Email ou senha incorretos");
    }

    private string GeneratesJwt(Guid userId)
    {
        var id = userId.ToString();
		var claims = User.Claims.ToList();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

        var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _jwtSettings.Issuer,
            Audience = _jwtSettings.Audience,
            Expires = DateTime.UtcNow.AddHours(_jwtSettings.Duration),
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        });

        var encodedToken = tokenHandler.WriteToken(token);

        return encodedToken;
    }
}
