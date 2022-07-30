using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// We want to authenticate users of our API using tokens issued by the IdentityServer project.
// We add JWT Bearer authentication services to the Service Collection to allow for
// dependency injection (DI), and configure Bearer as the default Authentication Scheme.
// Microsoft.AspNetCore.Authentication.JwtBearer Authentication middleware will:
// - Find and parse a JWT sent with incoming requests as an Authorization: Bearer header.
// - Validate the JWT’s signature to ensure that it was issued by IdentityServer.
// - Validate that the JWT is not expired.
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";

        options.TokenValidationParameters = new TokenValidationParameters
        {
            // Audience validation is disabled here because access to the api is modeled with ApiScopes only.
            // By default, no audience will be emitted unless the api is modeled with ApiResources instead.
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Add an Authorization Policy to the API that will check for the presence of the “api1” scope
    // in the access token. The protocol ensures that this scope will only be in the token
    // if the client requests it and IdentityServer allows the client to have that scope
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        // The IdentityServer was configured to allow this access
        // by including it in the allowedScopes property in Config.cs
        policy.RequireClaim("scope", "api1");
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// UseAuthentication() and UseAuthorization() should be in this order.

// Adds the authentication middleware to the pipeline
// so authentication will be performed automatically on every call into the host.
app.UseAuthentication();
// Adds the authorization middleware to make
// sure our API endpoint cannot be accessed by anonymous clients.
app.UseAuthorization();

// Enforce "ApiScope" policy defined above for all endpoints in all controllers.
// Alternatively, this could be enforced on a controller or action level.
app.MapControllers().RequireAuthorization("ApiScope");

app.Run();
