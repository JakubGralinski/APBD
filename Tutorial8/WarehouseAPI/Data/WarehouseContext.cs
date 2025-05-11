using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Models;

namespace WarehouseAPI.Data
{
    public class WarehouseContext : DbContext
    {
        public WarehouseContext(DbContextOptions<WarehouseContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductWarehouse> ProductWarehouses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductWarehouse>()
                .HasOne(pw => pw.Warehouse)
                .WithMany()
                .HasForeignKey(pw => pw.IdWarehouse);

            modelBuilder.Entity<ProductWarehouse>()
                .HasOne(pw => pw.Product)
                .WithMany()
                .HasForeignKey(pw => pw.IdProduct);

            modelBuilder.Entity<ProductWarehouse>()
                .HasOne(pw => pw.Order)
                .WithMany()
                .HasForeignKey(pw => pw.IdOrder);
        }
    }
} 