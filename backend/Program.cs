using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ReceiptTracker.Application.Services.Auth;
using ReceiptTracker.Application.Services.Receipts;
using ReceiptTracker.Application.Services.Users;
using ReceiptTracker.Infrastructure.Data;
using ReceiptTracker.Infrastructure.Parsers;
using ReceiptTracker.Infrastructure.Repositories.Receipts;
using ReceiptTracker.Infrastructure.Repositories.Users;
using System.Text;


var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load();

// Add EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program));

// Add Authentication

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });

builder.Services.AddAuthorization();


// Add controllers
builder.Services.AddControllers();

// Dependency injection for repositories & services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();
builder.Services.AddScoped<IReceiptParser, OpenAiReceiptParser>();

// Add CORS (optional, useful for frontend testing)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowAnyOrigin();
    });
});

// Authentication setup will come soon (JWT)
var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();
app.UseStaticFiles();
app.MapControllers();
app.Run();
