using Microsoft.EntityFrameworkCore;
using ReceiptTracker.Data;

var builder = WebApplication.CreateBuilder(args);

// Add EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add controllers
builder.Services.AddControllers();

// Add JWT / authentication setup will come soon

var app = builder.Build();

app.UseCors(p => p.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
app.MapControllers();
app.Run();
