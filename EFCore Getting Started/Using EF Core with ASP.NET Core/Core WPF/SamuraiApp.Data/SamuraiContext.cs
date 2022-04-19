using System;
using System.Configuration;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
  public class SamuraiContext : DbContext
  {
    public DbSet<Samurai> Samurais { get; set; }
    public DbSet<Battle> Battles { get; set; }
    public DbSet<Quote> Quotes { get; set; }
    public DbSet<SamuraiBattle> SamuraiBattles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {

      modelBuilder.Entity<SamuraiBattle>()
        .HasKey(s => new { s.BattleId, s.SamuraiId });

      foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
        modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModified");
        modelBuilder.Entity(entityType.Name).Ignore("IsDirty"); 
      }
    }

    public SamuraiContext(DbContextOptions options)
      :base(options) { }

    public SamuraiContext() { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
      if (!optionsBuilder.IsConfigured) {
        var settings = ConfigurationManager.ConnectionStrings;
        var connectionString = settings["productionDb"].ConnectionString;
        optionsBuilder.UseSqlServer(connectionString,
                                    options => options.MaxBatchSize(30));
        optionsBuilder.EnableSensitiveDataLogging();
      }
   }

    public override int SaveChanges() {
      foreach (var entry in ChangeTracker.Entries()
       .Where(e => e.State == EntityState.Added ||
                   e.State == EntityState.Modified)) {
        entry.Property("LastModified").CurrentValue = DateTime.Now;
     }
      return base.SaveChanges();
    }
  }
}