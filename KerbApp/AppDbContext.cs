using Microsoft.EntityFrameworkCore;

namespace KerbApp;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) { }
}