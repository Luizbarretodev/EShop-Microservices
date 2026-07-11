using EShop.CartApi.Models;
using Microsoft.EntityFrameworkCore;

namespace EShop.CartApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<CartItem> cartItems { get; set; }
    public DbSet<CartHeader> cartHeaders { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        //Product
        mb.Entity<Product>()
            .HasKey(c => c.Id);

        //Product
        mb.Entity<Product>()
            .Property(c => c.Id)
                .ValueGeneratedNever();

        //Definindo aqui para não poluir as models com data annotations
        mb.Entity<Product>().
            Property(c => c.Name).
                HasMaxLength(100).
                IsRequired();

        mb.Entity<Product>().
            Property(c => c.Description).
                HasMaxLength(255).
                IsRequired();

        mb.Entity<Product>().
            Property(c => c.ImageURL).
                HasMaxLength(255).
                IsRequired();

        mb.Entity<Product>().
            Property(c => c.CategoryName).
                HasMaxLength(100).
                IsRequired();

        mb.Entity<Product>().
            Property(c => c.Price).
                HasPrecision(12, 2);

        //CartHeader
        mb.Entity<CartHeader>().
            Property(c => c.UserId).
                HasMaxLength(255).
                IsRequired();

        mb.Entity<CartHeader>().
            Property(c => c.CoupounCode).
                HasMaxLength(100);
    }
}
