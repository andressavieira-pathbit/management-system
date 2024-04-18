using Management.System.Domain.Management.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace Management.System.Infrastructure.Repository;

[ExcludeFromCodeCoverage]
public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<CustomerEntity> Customers { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<OrderEntity> Orders { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("uuid-ossp");

        modelBuilder.Entity<UserEntity>()
            .Property(e => e.UserId)
            .HasDefaultValueSql("uuid_generate_v4()");

        modelBuilder.Entity<CustomerEntity>()
            .Property(e => e.CustomerId)
            .HasDefaultValueSql("uuid_generate_v4()");

        modelBuilder.Entity<ProductEntity>()
            .Property(e => e.ProductId)
            .HasDefaultValueSql("uuid_generate_v4()");

        modelBuilder.Entity<OrderEntity>()
            .Property(e => e.OrderId)
            .HasDefaultValueSql("uuid_generate_v4()");

        modelBuilder.Entity<UserEntity>()
       .Property(e => e.UserType)
       .HasConversion<int>();

        modelBuilder.Entity<OrderEntity>()
       .Property(e => e.Status)
       .HasConversion<int>();

        modelBuilder.Entity<OrderEntity>()
       .Property(e => e.OrderDate)
       .HasColumnType("timestamp");

        base.OnModelCreating(modelBuilder);
    }
}
