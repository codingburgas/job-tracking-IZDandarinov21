using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using JobTracking.DataAccess.Data;
using JobTracking.DataAccess.Interfaces;
using JobTracking.DataAccess.Repositories;
using JobTracking.Application.Interfaces;
using JobTracking.Application.Services;
using JobTracking.Application.Mappers; 
using JobTracking.Domain.Entities; 
using JobTracking.Domain.Enums; 
using Microsoft.AspNetCore.Authorization; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Конфигуриране на DbContext за SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Конфигуриране на AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

// Dependency Injection за Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Dependency Injection за Services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IJobAdvertisementService, JobAdvertisementService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            /
            policy.WithOrigins("https://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials(); 
        });
});

// Конфигуриране на JWT автентикация
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtSettings["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true; 
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidateAudience = true,
        ValidAudience = jwtSettings["Audience"],
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero // Токенът изтича точно в Expires времето
    };
});

// Конфигуриране на политики за авторизация
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("UserPolicy", policy => policy.RequireAuthenticatedUser().RequireRole(UserRole.User.ToString(), UserRole.Admin.ToString()));
    options.AddPolicy("AdminPolicy", policy => policy.RequireAuthenticatedUser().RequireRole(UserRole.Admin.ToString()));
});


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

