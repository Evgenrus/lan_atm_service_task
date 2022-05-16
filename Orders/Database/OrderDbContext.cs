using Microsoft.EntityFrameworkCore;
using Orders.Database.Entities;

namespace Orders.Database;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }


    public OrderDbContext(DbContextOptions<OrderDbContext> options) 
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ordersdb;Trusted_Connection=True;",
            builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Configuring foreign keys

        // modelBuilder.Entity<Customer>()
        //     .HasMany(a => a.Orders)
        //     .WithOne(b => b.Customer)
        //     .HasForeignKey(c => c.CustomerId);

        modelBuilder.Entity<Order>()
            .HasMany(a => a.OrderItems)
            .WithOne(b => b.Order)
            .HasForeignKey(c => c.OrderId);

        modelBuilder.Entity<Cart>()
            .HasMany(a => a.Items)
            .WithOne(b => b.Cart)
            .HasForeignKey(c => c.CartId);
    }
}