using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiAppCore.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SamuraiAppCore.Domain;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
    private DbContextOptions<SamuraiContext> _options;
    public UnitTest1()
    {
      _options = new DbContextOptionsBuilder<SamuraiContext>()
       .UseInMemoryDatabase().Options;
      SeedInMemoryStore();
    }
    [TestMethod]
    public void TwoPlusTwoEqualsFour()
    {
      Assert.AreEqual(4, 2 + 2);
    }
    [TestMethod]
    public void CanRetrieveListOfSamuraiValues()
    {
      var context = new SamuraiContext(_options);
      var repo = new DisconnectedData(context);
      var list = repo.GetSamuraiReferenceList();
      Assert.IsInstanceOfType(list,
        typeof(List<KeyValuePair<int, string>>));
    }

    [TestMethod]
    public void CanRetrieveAllSamuraiValuePairs()
    {
      var context = new SamuraiContext(_options);
      var repo = new DisconnectedData(context);
      Assert.AreEqual(2, repo.GetSamuraiReferenceList().Count);
    }

    [TestMethod]
    public void CanLoadQuotesAndIdentityForASamurai()
    {
      var context = new SamuraiContext(_options);
      var repo = new DisconnectedData(context);
      var graph = repo.LoadSamuraiGraph(1);
      Assert.AreEqual(2, graph.Quotes.Count);
      Assert.IsNotNull(graph.SecretIdentity);
    }

    private void SeedInMemoryStore()
    {
      using (var context = new SamuraiContext(_options))
      {
        if (!context.Samurais.Any())
        {
          context.Samurais.AddRange(
            new Samurai
            {
              Id = 1,
              Name = "Julie",
              SecretIdentity = new SecretIdentity { RealName = "Julia" },
              Quotes = new List<Quote>
                       {
                         new Quote {Text = "Howdy"},
                         new Quote {Text = "Choooocolaaaate"}
                       }
            },
            new Samurai
            {
              Id = 2,
              Name = "Giantpuppy",
              SecretIdentity = new SecretIdentity { RealName = "Sampson" },
              Quotes = new List<Quote>
                       {
                         new Quote {Text = "Woof"}
                       }
            }
          );
          context.SaveChanges();
        }
      }
    }
  }
}
