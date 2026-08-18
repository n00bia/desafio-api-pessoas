using Api_Pessoas.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api_Pessoas.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            if (request == null ||
               string.IsNullOrWhiteSpace(request.Username) ||
               string.IsNullOrWhiteSpace(request.Password))
            {
                return BadRequest(new
                {
                    mensagem = "Usuário e senha são obrigatórios."
                });
            }

            if (request.Username != "admin" ||
                request.Password != "admin")
            {
                return Unauthorized(new
                {
                    mensagem = "Usuário ou senha inválidos."
                });
            }

            var jwtKey = _configuration["Jwt:Key"];

            if (string.IsNullOrWhiteSpace(jwtKey))
            {
                return StatusCode(500, new
                {
                    mensagem = "Chave JWT não configurada."
                });
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, request.Username)
            };           

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            var tokenString = new JwtSecurityTokenHandler()
                .WriteToken(token);

            return Ok(new
            {
                token = tokenString
            });
        }
    }
}
