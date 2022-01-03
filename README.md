# JWT Multiple Audience POC
This project is a .NET 6 example to prove-out JWTs with multiple audiences. There seems to be some opposition to this concept
but according to the [JWT Specification](https://datatracker.ietf.org/doc/html/rfc7519#section-4.1.3), multiple audiences are the "general" case, where as a single audience is a "special" case. I wanted to leverage an array of audiences for a microservices environment where services are dependent on other services. Rather than getting access tokens for each
service, would it not make more sense to have a single jwt that is valid for each dependent service? This small poc demonstrates that a jwt with an audience claim containing multiple audience values can indeed be validated against a service expecting a specific audience.

## Getting Started
1. F5 to run project (you should see swagger)
2. Use the /auth endpoint to generate a jwt
3. Copy jwt
4. Click the "Authorize" button in swagger and plug in that jwt value
5. Execute the /weather-forecasts endpoint to verify you are authorized and get a 200 response

## Decoded JWT
The decoded jwt payload will look something like the following:
```json
{
  "aud": [
    "aud1",
    "aud2"
  ],
  "nbf": 1641227516,
  "exp": 1641231116,
  "iat": 1641227516,
  "iss": "me"
}
```

## Configuration
The jwt settings are configured in appsettings:
```json
"JwtAudiences": ["aud1", "aud2"],
"ValidAudience": "aud2"
```

## Token Validation
The validation of the jwt takes place in Extensions/AuthExtensions.cs:
```csharp
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
            ValidAudience = aud, // <-- change this value in appsettings to verify any listed audience works
            ValidateLifetime = true,
            RequireExpirationTime = true
        };
    });
```