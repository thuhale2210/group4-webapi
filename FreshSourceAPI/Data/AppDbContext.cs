using FreshSourceAPI.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace FreshSourceAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Supplier> Suppliers => Set<Supplier>();
        public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Supplier 1→* Products
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.Products)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Supplier 1→* PurchaseOrders
            modelBuilder.Entity<Supplier>()
                .HasMany(s => s.PurchaseOrders)
                .WithOne(o => o.Supplier)
                .HasForeignKey(o => o.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            // Product 1→* PurchaseOrders
            modelBuilder.Entity<Product>()
                .HasMany<PurchaseOrder>()
                .WithOne(po => po.Product)
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}