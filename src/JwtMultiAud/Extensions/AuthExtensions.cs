using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JwtMultiAud.Extensions
{
    public static class AuthExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var key = configuration.GetValue<string>("JwtSecurityKey");
            var iss = configuration.GetValue<string>("JwtIssuer");
            var aud = configuration.GetValue<string>("ValidAudience");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RequireSignedTokens = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                        ValidateIssuer = true,
                        ValidIssuer = iss,
                        ValidateAudience = true,
                        ValidAudience = aud,
                        ValidateLifetime = true,
                        RequireExpirationTime = true
                    };
                });
        }
    }
}