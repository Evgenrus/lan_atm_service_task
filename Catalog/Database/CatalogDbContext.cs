using Catalog.Database.Entity;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Database;

public class CatalogDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<ProductStock> Stocks { get; set; }

    public CatalogDbContext()
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Brand>()
            .HasMany(a => a.Products)
            .WithOne(b => b.Brand)
            .HasForeignKey(c => c.BrandId);

        modelBuilder.Entity<Category>()
            .HasMany(a => a.Products)
            .WithOne(b => b.Category)
            .HasForeignKey(c => c.CategoryId);

        modelBuilder.Entity<ProductStock>()
            .HasOne(a => a.Products)
            .WithOne(b => b.Stock)
            .HasForeignKey<Product>(c => c.ProductStockId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=catalogdb;Trusted_Connection=True;");
    }
}