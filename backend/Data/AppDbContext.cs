using Microsoft.EntityFrameworkCore;
using ReceiptTracker.Models;
using ReceiptTracker.Models.Receipts;

namespace ReceiptTracker.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) 
        : base(options) { }

    // tables
    public DbSet<User> Users => Set<User>();
    public DbSet<Receipt> Receipts => Set<Receipt>();
    public DbSet<ReceiptItem> ReceiptItems => Set<ReceiptItem>();

}
