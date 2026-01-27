using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using LoanSystem.API;
using LoanSystem.API.Middleware;
using LoanSystem.Application.Interface;
using LoanSystem.Application.Services;
using LoanSystem.Application.Validations;
using LoanSystem.Infrastructure.ApplicationDbContext;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "LoanSystem.API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "LoanSystem",
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "LoanSystem",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "SuperSecretKeyForJwtAuthentication123!"))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddAppDI();


// test configurations 
//using env for test
//builder.Services.AddDbContext<LoanSystemDbContext>(options =>
//{
//    var host = Environment.GetEnvironmentVariable("PGHOST");
//    var port = Environment.GetEnvironmentVariable("PGPORT");
//    var database = Environment.GetEnvironmentVariable("PGDATABASE");
//    var user = Environment.GetEnvironmentVariable("PGUSER");
//    var password = Environment.GetEnvironmentVariable("PGPASSWORD");
//    var connectionString = $"Host={host};Port={port};Database={database};Username={user};Password={password}";
//    options.UseNpgsql(connectionString);
//});

builder.Services.AddDbContext<LoanSystemDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

//builder.WebHost.UseUrls("http://0.0.0.0:5000");

var app = builder.Build();


// test auto read !!
//using (var scope = app.Services.CreateScope())
//{
//    var db = scope.ServiceProvider.GetRequiredService<LoanSystemDbContext>();
//    db.Database.Migrate();
//}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRequestLogging();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
