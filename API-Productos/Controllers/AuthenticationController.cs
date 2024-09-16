using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;

using API_Productos.models;

using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace API_Productos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly string secretkey;
        public AuthenticationController(IConfiguration config)
        {
            secretkey = config.GetSection("settings").GetSection("secretkey").ToString();

        }

        [HttpPost]
        [Route("Validar")]
        public IActionResult Validar([FromBody] Usuario request)
        {
            if (request.Correo == "Sandev@g.com" && request.Clave == "1234")
            {
                var KeyBytes = Encoding.ASCII.GetBytes(secretkey);
                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier,request.Correo));
                var TokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claims,
                    Expires = DateTime.UtcNow.AddMinutes(5),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(KeyBytes),SecurityAlgorithms.HmacSha256Signature),
                };

                var TokenHandler = new JwtSecurityTokenHandler();
                var TokenConfig = TokenHandler.CreateToken(TokenDescriptor);

                string TokenCreado = TokenHandler.WriteToken(TokenConfig);

                return StatusCode(StatusCodes.Status200OK, new { token = TokenCreado });


            }else
            {
                return StatusCode(StatusCodes.Status400BadRequest, new { token = "" });
            }
        }
    }
}
