using Microsoft.EntityFrameworkCore;
using ReceiptTracker.Data;
using ReceiptTracker.Repositories;
using ReceiptTracker.Services.Users;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();

// Dependency injection for repositories & services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

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

app.UseCors();
app.MapControllers();
app.Run();
