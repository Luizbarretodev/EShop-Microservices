using EShop.ProductApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;

namespace EShop.ProductApi.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }

    //Sobrescrevendo as convenções com Fluent Api
    //outra forma seria utilizar data annotations nos atributos
    protected override void OnModelCreating(ModelBuilder mb)
    {
        //Category
        mb.Entity<Category>().HasKey(c => c.CategoryId);

        mb.Entity<Category>()
            .Property(n => n.Name)
              .HasMaxLength(100)
                .IsRequired();

        mb.Entity<Category>()
            .HasMany(p => p.Products)
              .WithOne(c => c.Category)
                .IsRequired()
                  .OnDelete(DeleteBehavior.Restrict);

        //HasData é utilizado para popular as tabelas automaticamente quando a migration for criada
        //é util para dados padrão ou pré definidos
        mb.Entity<Category>().HasData(
           //Nesse caso é necessário informar o Id
            new Category { CategoryId = 1, Name = "Miscellaneous" },
            new Category { CategoryId = 2, Name = "School Supplies" },
            new Category { CategoryId = 3, Name = "Accessories" }
        );

        //Product
        mb.Entity<Product>()
            .Property(n => n.Name)
              .HasMaxLength(100)
                .IsRequired();

        mb.Entity<Product>()
            .Property(d => d.Description)
              .HasMaxLength(255)
                .IsRequired();

        mb.Entity<Product>()
            .Property(i => i.ImageURL)
              .HasMaxLength(255)
                .IsRequired();

        mb.Entity<Product>()
            .Property(p => p.Price)
              .HasPrecision(10, 2);
    }
}