using OxideClassLibrary.Contexts;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using OxideWebApi.Services;


var builder = WebApplication.CreateBuilder(args);

var secretJwtKey = new SymmetricSecurityKey(
    Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]!));

var connectionString 
    = builder.Configuration.GetConnectionString("OxideDbContext") 
    ?? throw new InvalidOperationException("Connection string 'OxideDbContext' not found.");

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

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();
