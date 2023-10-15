namespace FruitAPI.DataAccess.Context;

using FruitAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options) { }

    public DbSet<Fruit> Fruits { get; set; }
    public DbSet<FruitType> FruitTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Fruit>()
            .HasOne(c => c.Type)
            .WithMany(s => s.Fruits)
            .HasForeignKey(c => c.TypeId);
    }
}

