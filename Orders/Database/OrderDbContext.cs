using Microsoft.EntityFrameworkCore;
using Orders.Database.Entities;

namespace Orders.Database;

public class OrderDbContext : DbContext
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=ordersdb;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //Configuring foreign keys
        modelBuilder.Entity<Customer>()
            .HasMany(a => a.Orders)
            .WithOne(b => b.Customer)
            .HasForeignKey(c => c.CustomerId);

        modelBuilder.Entity<Order>()
            .HasMany(a => a.OrderItems)
            .WithOne(b => b.Order)
            .HasForeignKey(c => c.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(a => a.Item)
            .WithMany(b => b.OrderItems)
            .HasForeignKey(c => c.ItemId);

        modelBuilder.Entity<Cart>()
            .HasMany(a => a.Items)
            .WithOne(b => b.Cart)
            .HasForeignKey(c => c.CartId);

        modelBuilder.Entity<CartItem>()
            .HasOne(a => a.Item)
            .WithMany(b => b.CartItems)
            .HasForeignKey(c => c.CartItemId);
    }
}