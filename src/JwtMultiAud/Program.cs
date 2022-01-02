using JwtMultiAud.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerWithJwtValidation();
builder.Services.AddJwtAuthentication(builder.Configuration);

var app = builder.Build();
app.UseSwaggerWithOptions();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();