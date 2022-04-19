using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public SamuraiContext(DbContextOptions<SamuraiContext> options)
               : base(options)
        {
        }
        public SamuraiContext()
        {

        }
        public static readonly LoggerFactory MyConsoleLoggerFactory
           = new LoggerFactory(new[] {
              new ConsoleLoggerProvider((category, level)
                => category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Information, true) });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLoggerFactory(MyConsoleLoggerFactory)
                    .UseSqlServer(
                     "Server = (localdb)\\mssqllocaldb; Database = SamuraiAppData; Trusted_Connection = True; ")
                     .EnableSensitiveDataLogging(true);
            }
        }

        [DbFunction(Schema ="dbo")]
        public static string EarliestBattleFoughtBySamurai(int samuraiId)
        {
              throw new Exception();
        }

       
        [DbFunction(Schema = "dbo")]
        public  static int DaysInBattle(DateTime start, DateTime end)
        {
            return (int)end.Subtract(start).TotalDays + 1;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
               .HasKey(s => new { s.SamuraiId, s.BattleId });
            modelBuilder.Entity<Battle>().Property(b => b.StartDate).HasColumnType("Date");
            modelBuilder.Entity<Battle>().Property(b => b.EndDate).HasColumnType("Date");
                 foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.Entity(entityType.Name).Property<DateTime>("Created");
                modelBuilder.Entity(entityType.Name).Property<DateTime>("LastModified");
            }
             modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(b => b.GivenName).HasColumnName("GivenName");
            modelBuilder.Entity<Samurai>().OwnsOne(s => s.BetterName).Property(b => b.SurName).HasColumnName("SurName");
           

        }

#if false
//the Owned Type work-around does not work with InMemory provider!!
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            var timestamp = DateTime.Now;
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => (e.State == EntityState.Added || e.State == EntityState.Modified)
                             && !e.Metadata.IsOwned()))
            {
                entry.Property("LastModified").CurrentValue = timestamp;
                if (entry.State==EntityState.Added)
                {
                    entry.Property("Created").CurrentValue = timestamp;
                }

                if (entry.Entity is Samurai)
                {
                    if (entry.Reference("BetterName").CurrentValue == null)
                    {
                        entry.Reference("BetterName").CurrentValue = PersonFullName.Empty();
                    }
                    entry.Reference("BetterName").TargetEntry.State = entry.State;
                }
            }
            return base.SaveChanges();
        }
#endif
    }
}
 