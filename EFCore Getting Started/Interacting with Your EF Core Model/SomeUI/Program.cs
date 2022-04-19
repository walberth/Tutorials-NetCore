using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SomeUI
{
  internal class Program
  {
    private static SamuraiContext _context = new SamuraiContext();

    private static void Main(string[] args)
    {
      _context.Database.EnsureCreated();
      _context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());

       InsertNewPkFkGraph();


      #region inactive methods

      // InsertNewPkFkGraphMultipleChildren();
      // InsertNewOneToOneGraph();
      //AddChildToExistingObjectWhileTracked();
      //AddOneToOneToExistingObjectWhileTracked();
      //AddBattles();
      //AddManyToManyWithFks();
      //EagerLoadWithInclude();
      //EagerLoadManyToManyAkaChildrenGrandchildren();
      //EagerLoadFilteredManyToManyAkaChildrenGrandchildren();
      //EagerLoadWithMultipleBranches();
      //EagerLoadWithFromSql();
      //EagerLoadViaProjection();
      //EagerLoadAllOrNothingChildrenNope();
      //FilterAcrossRelationship();
      //ExplicitLoad();
      //  ExplicitLoadWithChildFilter();
      //AnonymousTypeViaProjection();
      //AnonymousTypeViaProjectionWithRelated();
      //RelatedObjectsFixUp();
      //EagerLoadViaProjectionNotQuite();
      //EagerLoadViaProjectionAndScalars();
      //FilteredEagerLoadViaProjectionNope();

      #endregion
    }

    private static void InsertNewPkFkGraph() {
      var samurai = new Samurai
      {
        Name = "Kambei Shimada",
        Quotes = new List<Quote>
                               {
                                 new Quote {Text = "I've come to save you"}
                               }
      };
      _context.Samurais.Add(samurai);
      _context.SaveChanges();
    }

    private static void InsertNewPkFkGraphMultipleChildren() {
      var samurai = new Samurai
      {
        Name = "Kyūzō",
        Quotes = new List<Quote> {
          new Quote {Text = "Watch out for my sharp sword!"},
          new Quote {Text="I told you to watch out for the sharp sword! Oh well!" }
        }
      };
      _context.Samurais.Add(samurai);
      _context.SaveChanges();
    }

    private static void InsertNewOneToOneGraph() {
      var samurai = new Samurai { Name = "Shichirōji " };
      samurai.SecretIdentity = new SecretIdentity { RealName = "Julie" };
      _context.Add(samurai);
      _context.SaveChanges();
    }

    private static void AddChildToExistingObjectWhileTracked() {
      var samurai = _context.Samurais.First();
      samurai.Quotes.Add(new Quote {
                           Text = "I bet you're happy that I've saved you!"
                         });
      _context.SaveChanges();
    }

    private static void AddOneToOneToExistingObjectWhileTracked() {
      var samurai = _context.Samurais
        .FirstOrDefault(s => s.SecretIdentity == null);
      samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
      _context.SaveChanges();
    }
    
    private static void AddBattles()  {
      _context.Battles.AddRange(
        new Battle {Name = "Battle of Shiroyama",StartDate = new DateTime(1877, 9, 24),EndDate = new DateTime(1877, 9, 24)},
        new Battle {Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31)},
        new Battle {Name = "Boshin War", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1)}
        );
      _context.SaveChanges();
    }

    private static void AddManyToManyWithFks()
    { _context=new SamuraiContext();
      var sb = new SamuraiBattle {SamuraiId = 1, BattleId = 1};
      _context.SamuraiBattles.Add(sb);
      _context.SaveChanges();
    }

    private static void AddManyToManyWithObjects() {
      _context = new SamuraiContext();
      var samurai = _context.Samurais.FirstOrDefault();
      var battle = _context.Battles.FirstOrDefault();
      _context.SamuraiBattles.Add(
       new SamuraiBattle { Samurai = samurai, Battle = battle });
      _context.SaveChanges();
    }

    private static void EagerLoadWithInclude() {
      _context = new SamuraiContext();
      var samuraiWithQuotes = _context.Samurais.Include(s => s.Quotes).ToList();
    }

    private static void EagerLoadManyToManyAkaChildrenGrandchildren() {
      _context = new SamuraiContext();
      var samuraiWithBattles = _context.Samurais
        .Include(s => s.SamuraiBattles)
        .ThenInclude(sb => sb.Battle).ToList();
    }
    private static void EagerLoadFilteredManyToManyAkaChildrenGrandchildren() {
      _context = new SamuraiContext();
      var samuraiWithBattles = _context.Samurais
        .Include(s => s.SamuraiBattles)
        .ThenInclude(sb => sb.Battle)
        .Where(s=>s.Name== "Kyūzō").ToList();
    }

    private static void EagerLoadWithMultipleBranches() {
      _context = new SamuraiContext();
      var samurais = _context.Samurais
        .Include(s => s.SecretIdentity)
        .Include(s => s.Quotes).ToList();
    }

    private static void EagerLoadWithFindNope() {

    }
    
    private static void EagerLoadWithFromSql() {
      var samurais = _context.Samurais.FromSql("select * from samurais")
        .Include(s => s.Quotes)
        .ToList();
    }

    private static void EagerLoadFilterChildrenNope()
    { //this won't work. No filtering, no sorting on Include
      _context=new SamuraiContext();
      var samurais = _context.Samurais
        .Include(s => s.Quotes.Where(q => q.Text.Contains("happy")))
        .ToList();
    }



    private static void AnonymousTypeViaProjection() {
      _context = new SamuraiContext();
      var quotes = _context.Quotes
        .Select(q => new { q.Id, q.Text })
        .ToList();
    }

    private static void AnonymousTypeViaProjectionWithRelated() {
      _context = new SamuraiContext();
      var samurais = _context.Samurais
        .Select(s => new {
          s.Id,
          s.SecretIdentity.RealName,
          QuoteCount = s.Quotes.Count
        })
        .ToList();
    }

    private static void RelatedObjectsFixUp()
    {
      _context=new SamuraiContext();
      var samurai = _context.Samurais.Find(1);
      var quotes = _context.Quotes.Where(q => q.SamuraiId == 1).ToList();
    }

    private static void EagerLoadViaProjectionNotQuite() {
      _context = new SamuraiContext();
      var samurais = _context.Samurais
        .Select(s => new { Samurai = s, Quotes = s.Quotes })
        .ToList();
      //all results are in memory, but navigations are not fixed up
      //watch this github issue:https://github.com/aspnet/EntityFramework/issues/7131
    }
    private static void EagerLoadViaProjectionAndScalars() {
      _context = new SamuraiContext();
      var samurais = _context.Samurais
        .Select(s => new { s.Id, Quotes = s.Quotes })
        .ToList();
         }

    private static void FilteredEagerLoadViaProjectionNope() {
      _context = new SamuraiContext();
      var samurais = _context.Samurais
        .Select(s => new
        {
          Samurai = s,
          Quotes = s.Quotes
                    .Where(q => q.Text.Contains("happy"))
                    .ToList()
        })
        .ToList();
      //quotes are not even retrieved in query.
      //https://github.com/aspnet/EntityFramework/issues/7131
    }


     
    private static void ExplicitLoad() {
      _context=new SamuraiContext();
      var samurai = _context.Samurais.FirstOrDefault();
      _context.Entry(samurai).Collection(s => s.Quotes).Load();
      _context.Entry(samurai).Reference(s => s.SecretIdentity).Load();

    }

    private static void ExplicitLoadWithChildFilter() {
      _context = new SamuraiContext();
      var samurai = _context.Samurais.FirstOrDefault();
      // _context.Entry(samurai)
      //   .Collection(s => s.Quotes.Where(q=>q.Text.Contains("happy"))).Load();

      _context.Entry(samurai)
        .Collection(s => s.Quotes)
         .Query()
        .Where(q => q.Text.Contains("happy"))
        .Load();


    }


    private static void FilterAcrossRelationship()
    {
      _context=new SamuraiContext();
      var samurais = _context.Samurais
        .Where(s => s.Quotes.Any(q => q.Text.Contains("happy")))
        .ToList();
    }


   
   
  }
}