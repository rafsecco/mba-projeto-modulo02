using Api.Models;
using Core.Services;
using Core.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly JwtSettings _jwtSettings;

        public UsersController(IOptions<JwtSettings> jwtSettings, IUserService userService)
        {
            _userService = userService;

            _jwtSettings = jwtSettings.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Register(UserViewModel userViewModel, CancellationToken cancellationToken)
        {
            await _userService.RegisterAsync(userViewModel, cancellationToken);
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(UserViewModel userViewModel, CancellationToken cancellationToken)
        {
            var userId = await _userService.LoginAsync(userViewModel, cancellationToken);
            if (userId.HasValue)
            {
                return Ok(new { token = GeneratesJwt(userId.Value) });
            }
            return Problem("Incorrect password or email");
        }

        private string GeneratesJwt(Guid userId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Sid, userId.ToString())
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience,
                Expires = DateTime.UtcNow.AddHours(_jwtSettings.Duration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            });

            var encodedToken = tokenHandler.WriteToken(token);

            return encodedToken;
        }
    }
}