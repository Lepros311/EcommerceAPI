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
        modelBuilder.Entity<Category>().ToTable("Categories");
        modelBuilder.Entity<Product>().ToTable("Products");
        modelBuilder.Entity<Sale>().ToTable("Sales");

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .IsRequired();

        modelBuilder.Entity<LineItem>().ToTable("LineItems");

        modelBuilder.Entity<LineItem>()
            .HasOne(li => li.Sale)
            .WithMany(s => s.LineItems)
            .HasForeignKey(li => li.SaleId);

        modelBuilder.Entity<LineItem>()
            .HasOne(li => li.Product)
            .WithMany(p => p.LineItems)
            .HasForeignKey(li => li.ProductId);

        modelBuilder.Entity<LineItem>()
            .Property(li => li.UnitPrice)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Sale>()
            .Property(s => s.Total)
            .HasColumnType("decimal(18,2)");


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
                    TimeStamp = new DateTime(2025, 08, 01, 12, 37, 22),
                    Total = 22.99m
                },
                new Sale
                {
                    Id = 2,
                    TimeStamp = new DateTime(2025, 08, 03, 14, 30, 48),
                    Total = 61.20m
                },
                new Sale
                {
                    Id = 3,
                    TimeStamp = new DateTime(2025, 08, 07, 11, 39, 10),
                    Total = 156.20m
                },
                new Sale
                {
                    Id = 4,
                    TimeStamp = new DateTime(2025, 08, 07, 19, 13, 55),
                    Total = 107.50m
                },
                new Sale
                {
                    Id = 5,
                    TimeStamp = new DateTime(2025, 08, 08, 9, 04, 17),
                    Total = 95.80m
                }
            });

        modelBuilder.Entity<LineItem>().HasData(
            new LineItem { Id = 1, SaleId = 1, ProductId = 8, Quantity = 1, UnitPrice = 22.99m },
            new LineItem { Id = 2, SaleId = 2, ProductId = 6, Quantity = 1, UnitPrice = 45.95m },
            new LineItem { Id = 3, SaleId = 2, ProductId = 3, Quantity = 1, UnitPrice = 15.25m },
            new LineItem { Id = 4, SaleId = 3, ProductId = 7, Quantity = 1, UnitPrice = 34.75m },
            new LineItem { Id = 5, SaleId = 3, ProductId = 6, Quantity = 1, UnitPrice = 45.95m },
            new LineItem { Id = 6, SaleId = 3, ProductId = 1, Quantity = 1, UnitPrice = 75.50m },
            new LineItem { Id = 7, SaleId = 4, ProductId = 2, Quantity = 2, UnitPrice = 53.75m },
            new LineItem { Id = 8, SaleId = 5, ProductId = 4, Quantity = 2, UnitPrice = 10.15m },
            new LineItem { Id = 9, SaleId = 5, ProductId = 1, Quantity = 1, UnitPrice = 75.50m }
        );

    }
}
