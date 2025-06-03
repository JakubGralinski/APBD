using Microsoft.EntityFrameworkCore;

namespace MockTest_test2.Database;

public class AppDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}