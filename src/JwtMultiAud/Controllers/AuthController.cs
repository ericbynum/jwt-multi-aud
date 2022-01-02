using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtMultiAud.Controllers;

[ApiController]
public class AuthController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("auth")]
    public IActionResult Authenticate()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Guid.NewGuid().ToString();
        var tokenKey = Encoding.ASCII.GetBytes(key);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = "ebynum",
            Subject = new ClaimsIdentity(
                new Claim[]
                {
                    new Claim(ClaimTypes.Name, "eric"),
                    new Claim("aud", "aud1"),
                    new Claim("aud", "aud2"),
                }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };
        
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(jwt);
    }
}