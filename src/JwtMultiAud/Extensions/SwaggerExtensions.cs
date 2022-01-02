using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace JwtMultiAud.Extensions;

public static class SwaggerExtensions
{
    public static void AddSwaggerWithJwtValidation(this IServiceCollection services)
    {
        services.AddSwaggerGen(ConfigureJwtValidation);
    }

    private static void ConfigureJwtValidation(this SwaggerGenOptions options)
    {
        var definitionName = "jwt";
        
        options.AddSecurityDefinition(definitionName, new OpenApiSecurityScheme
        {
            Description = "Adds a JWT Authorization header using the Bearer scheme.",
            Scheme = "bearer",
            Type = SecuritySchemeType.Http,
            In = ParameterLocation.Header
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = definitionName,
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });
    }

    public static void UseSwaggerWithOptions(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment()) return;

        app.UseSwagger();
        app.UseSwaggerUI(options => 
        {
            options.EnableTryItOutByDefault();
            options.EnablePersistAuthorization();
        });
    }
}