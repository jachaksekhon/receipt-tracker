using Microsoft.EntityFrameworkCore;
using ReceiptTracker.Application.Services.Auth;
using ReceiptTracker.Application.Services.Receipts;
using ReceiptTracker.Application.Services.Users;
using ReceiptTracker.Data;
using ReceiptTracker.Repositories.Receipts;
using ReceiptTracker.Repositories.Users;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddAutoMapper(typeof(Program));


// Add controllers
builder.Services.AddControllers();

// Dependency injection for repositories & services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IReceiptService, ReceiptService>();

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
app.UseStaticFiles();
app.MapControllers();
app.Run();
