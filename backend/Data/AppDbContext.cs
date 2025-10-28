using Microsoft.EntityFrameworkCore;
using ReceiptTracker.Models;

namespace ReceiptTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }


    // tables
    public DbSet<User> Users => Set<User>();
}
