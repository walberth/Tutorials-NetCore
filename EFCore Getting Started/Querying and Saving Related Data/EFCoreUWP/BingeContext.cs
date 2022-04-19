using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Windows.Storage;

namespace EFCoreUWP {

  public class BingeContext : DbContext {
    public DbSet<CookieBinge> Binges { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options) {
      var databaseFilePath = "CookieBinge.db";
      try {
        databaseFilePath = Path.Combine(
          ApplicationData.Current.LocalFolder.Path, databaseFilePath);
      }
      catch (InvalidOperationException) {
      }

       options.UseSqlite($"Data source={databaseFilePath}");
    }
  }
}