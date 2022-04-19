using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace UnitTestProject {
  [TestClass]
  public class SamuraiTests {

    [TestMethod]
    public void CanInsertSamuraiIntoDatabase() {
      var optionsBuilder = new DbContextOptionsBuilder();
      optionsBuilder.UseSqlServer
        ("Server = (localdb)\\mssqllocaldb; Database = TestDb; Trusted_Connection = True; ");
      using (var context = new SamuraiContext(optionsBuilder.Options))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        var samurai = new Samurai();
        Debug.WriteLine($"Default Samurai Id {samurai.Id}");
        context.Samurais.Add(samurai);
        var efDefaultId = samurai.Id;
        Debug.WriteLine($"EF Default Samurai Id {efDefaultId}");
        context.SaveChanges();
        Debug.WriteLine($"DB assigned Samurai Id {samurai.Id}");
        Assert.AreNotEqual(efDefaultId, samurai.Id);
      }
    }

    [TestMethod]
    public void CanInsertSamuraiWithSaveChanges() {
      var optionsBuilder=new DbContextOptionsBuilder();
      optionsBuilder.UseInMemoryDatabase();
      using (var context = new SamuraiContext(optionsBuilder.Options)) {
        var samurai = new Samurai();
        samurai.Name = "Julie";
        context.Samurais.Add(samurai);
        context.SaveChanges();
      }
      using (var context2 = new SamuraiContext(optionsBuilder.Options)) {
        Assert.AreEqual(1, context2.Samurais.Count());
      }
    }
  }
}