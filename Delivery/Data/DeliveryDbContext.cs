using Delivery.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Delivery.Data;

public class DeliveryDbContext : DbContext
{
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<DeliveryItem> DeliveryItems { get; set; }
    public DbSet<OrderDelivery> Deliveries { get; set; }

    public DeliveryDbContext(DbContextOptions<DeliveryDbContext> options)
        : base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        //configuring relations
        //One Courier to Many OrderDelivery
        modelBuilder.Entity<Courier>()
            .HasMany(x => x.Deliveries)
            .WithOne(x => x.Courier)
            .HasForeignKey(x => x.CourierId);

        // modelBuilder.Entity<OrderDelivery>()
        //     .HasOne(x => x.Courier)
        //     .WithMany(x => x.Deliveries)
        //     .HasForeignKey(x => x.CourierId);
        
        //One OrderDelivery to Many DeliveryItems
        modelBuilder.Entity<OrderDelivery>()
            .HasMany(x => x.OrderedItems)
            .WithOne(x => x.OrderDelivery)
            .HasForeignKey(x => x.OrderDeliveryId);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=deliverydb;Trusted_Connection=True;",
                builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
    }
}