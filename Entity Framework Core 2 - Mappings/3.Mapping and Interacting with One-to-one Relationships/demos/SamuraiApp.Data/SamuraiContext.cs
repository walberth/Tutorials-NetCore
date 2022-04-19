using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext:DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }
   

        public static readonly LoggerFactory MyConsoleLoggerFactory
           = new LoggerFactory(new[] {
              new ConsoleLoggerProvider((category, level)
                => category == DbLoggerCategory.Database.Command.Name
               && level == LogLevel.Information, true) });

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(MyConsoleLoggerFactory)
                .UseSqlServer(
                 "Server = (localdb)\\mssqllocaldb; Database = SamuraiAppData; Trusted_Connection = True; ")
                 .EnableSensitiveDataLogging(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
             modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new { s.SamuraiId, s.BattleId });
          
            ///Mapping nullable foreign key SamuraiId 
            //modelBuilder.Entity<Samurai>()
            //    .HasOne(s => s.SecretIdentity)
            //    .WithOne(i => i.Samurai).HasForeignKey<SecretIdentity>("SamuraiId");

            ///Mapping unconventionally named foreign key property
            /// Special syntax (parameterless WithOne, HFK<SecretIdentity>
            /// are because I have no Samurai navigation property
            //modelBuilder.Entity<Samurai>()
            //    .HasOne(i => i.SecretIdentity)
            //    .WithOne()
            //    .HasForeignKey<SecretIdentity>(i => i.SamuraiFK);
        }

    }
 }

