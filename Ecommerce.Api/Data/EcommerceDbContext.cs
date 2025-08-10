using Ecommerce.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Api.Data;

public class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Sale> Sales { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=EcommerceDb;Trusted_Connection=True;Initial Catalog=EcommerceDb");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .IsRequired();

        modelBuilder.Entity<Product>()
            .HasMany(p => p.Sales)
            .WithMany(s => s.Products)
            .UsingEntity(j => j.ToTable("ProductSales"));

        modelBuilder.Entity<Category>()
            .HasData(new List<Category>
            {
                new Category
                {
                    Id = 1,
                    Name = "Shoes",
                },
                new Category
                {
                    Id = 2,
                    Name = "Socks",
                },
                new Category
                {
                    Id = 3,
                    Name = "Pants",
                },
                new Category
                {
                    Id = 4,
                    Name = "Shirts",
                }
            });

        modelBuilder.Entity<Product>()
           .HasData(new List<Product>
           {
                new Product
                {
                    Id = 1,
                    Name = "Hightop Sneakers",
                    Price = 75.50m,
                    CategoryId = 1,
                },
                new Product
                {
                    Id = 2,
                    Name = "Boat Loafers",
                    Price = 53.75m,
                    CategoryId = 1,
                },
                new Product
                {
                    Id = 3,
                    Name = "Dress Socks",
                    Price = 15.25m,
                    CategoryId = 2,
                },
                new Product
                {
                    Id = 4,
                    Name = "Ankle Socks",
                    Price = 10.15m,
                    CategoryId = 2,
                },
                new Product
                {
                    Id = 5,
                    Name = "Dress Slacks",
                    Price = 35.99m,
                    CategoryId = 3,
                },
                new Product
                {
                    Id = 6,
                    Name = "Stonewash Jeans",
                    Price = 45.95m,
                    CategoryId = 3,
                },
                new Product
                {
                    Id = 7,
                    Name = "Flannel Shirt",
                    Price = 34.75m,
                    CategoryId = 4,
                },
                new Product
                {
                    Id = 8,
                    Name = "Shortsleeve Polo",
                    Price = 22.99m,
                    CategoryId = 4,
                }
           });

        modelBuilder.Entity<Sale>()
            .HasData(new List<Sale>
            {
                new Sale
                {
                    Id = 1,
                    TimeStamp = DateTime.Now,
                    Total = 22.99m
                },
                new Sale
                {
                    Id = 2,
                    TimeStamp = DateTime.Now,
                    Total = 61.20m
                },
                new Sale
                {
                    Id = 3,
                    TimeStamp = DateTime.Now,
                    Total = 156.20m
                },
                new Sale
                {
                    Id = 4,
                    TimeStamp = DateTime.Now,
                    Total = 20.30m
                },
                new Sale
                {
                    Id = 5,
                    TimeStamp = DateTime.Now,
                    Total = 53.75m
                }
            });

        modelBuilder.Entity("ProductSales").HasData(
            new { ProductsId = 8, SalesId = 1 },
            new { ProductsId = 6, SalesId = 2 },
            new { ProductsId = 3, SalesId = 2 },
            new { ProductsId = 7, SalesId = 3 },
            new { ProductsId = 6, SalesId = 3 },
            new { ProductsId = 1, SalesId = 3 }
            );
    }
}
