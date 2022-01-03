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
    private readonly IConfiguration configuration;

    public AuthController(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    [AllowAnonymous]
    [HttpPost("auth")]
    public IActionResult Authenticate()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = configuration.GetValue<string>("JwtSecurityKey");
        var tokenKey = Encoding.ASCII.GetBytes(key);

        var audiences = configuration.GetSection("JwtAudiences").Get<string[]>();
        var audClaims = audiences.Select(aud => new Claim(JwtRegisteredClaimNames.Aud, aud));

        var claims = new List<Claim>();
        // adding multiple "aud" claim keys will become a single "aud" claim with an array of values
        claims.AddRange(audClaims);

        var tokenDescriptor = new SecurityTokenDescriptor()
        {
            Issuer = configuration.GetValue<string>("JwtIssuer"),
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var jwt = tokenHandler.WriteToken(token);

        return Ok(jwt);
    }
}