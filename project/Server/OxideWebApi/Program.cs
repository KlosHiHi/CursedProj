using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OxideWebApi.Contexts;
using OxideWebApi.Services;
using Scalar.AspNetCore;
using System.Security.Claims;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

var secretJwtKey = new SymmetricSecurityKey(
    Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!));

var connectionString
    = builder.Configuration.GetConnectionString("OxideDbConnection")
    ?? throw new InvalidOperationException("Connection string 'OxideDbConnection' not found.");

builder.Services.AddDbContext<OxideDbContext>(options => options.UseMySQL(connectionString));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = secretJwtKey,

            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],

            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],

            ValidateLifetime = true,
            RequireExpirationTime = true,
            RequireSignedTokens = true,
            ValidAlgorithms = [SecurityAlgorithms.HmacSha256],

            ClockSkew = TimeSpan.Zero,
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<LoginService>();

builder.Services.AddControllers();
builder.Services.AddOpenApi();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.MapGet("/api", () =>
{
    return Results.Ok(new
    {
        Message = "API работает, ура!",
        Date = DateTime.UtcNow
    });
});

app.Run();
