using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Linq;

namespace SomeUI
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            //CreateSamurai();
            //RetrieveSamuraisCreatedInPastWeek();
            //CreateThenEditSamuraiWithQuote();
            //GetAllSamurais();
            //CreateSamuraiWithBetterName();
            //RetrieveAndUpdateBetterName();
            //CreateAndFixUpNullBetterName();
            ReplaceBetterName();
        }

        private static void CreateSamuraiWithBetterName()
        {
            var samurai = new Samurai
            {
                Name = "Jack le Black",
                BetterName = PersonFullName.Create("Jack", "Black")
            };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void CreateAndFixUpNullBetterName()
        {
            _context.Samurais.Add(new Samurai { Name = "Chrisjen" });
            _context.SaveChanges();
            _context = new SamuraiContext();
            var persistedSamurai = _context.Samurais.FirstOrDefault(s => s.Name == "Chrisjen");
            if (persistedSamurai is null) { return; }
            if (persistedSamurai.BetterName.IsEmpty())
            {
                persistedSamurai.BetterName = null;
            }
        }

        private static void ReplaceBetterName()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Chrisjen");
            _context.Entry(samurai).Reference(s => s.BetterName).TargetEntry.State = EntityState.Detached;
            samurai.BetterName = PersonFullName.Create("Shohreh", "Aghdashloo");
            _context.Samurais.Update(samurai);
            _context.SaveChanges();
        }

        //private static void RetrieveAndUpdateBetterName()
        //{
        //    var samurai = _context.Samurais.FirstOrDefault(s => s.BetterName.SurName == "Black");
        //    samurai.BetterName.GivenName = "Jill";
        //    _context.SaveChanges();
        //}

        private static void GetAllSamurais()
        {
            var allsamurais = _context.Samurais.ToList();
        }

        private static void RetrieveSamuraisCreatedInPastWeek()
        {
            var oneWeekAgo = DateTime.Now.AddDays(-7);
            //var newSamurais = _context.Samurais
            //                          .Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo)
            //                          .ToList();
            var samuraisCreated = _context.Samurais
                                        .Where(s => EF.Property<DateTime>(s, "Created") >= oneWeekAgo)
                                        .Select(s => new { s.Id, s.Name, Created = EF.Property<DateTime>(s, "Created") })
                                        .ToList();
        }

        private static void CreateSamurai()
        {
            var samurai = new Samurai { Name = "Ronin" };
            _context.Samurais.Add(samurai);
            _context.Entry(samurai).Property("Created").CurrentValue = DateTime.Now;
            _context.Entry(samurai).Property("LastModified").CurrentValue = DateTime.Now;
            _context.SaveChanges();
        }

        private static void CreateThenEditSamuraiWithQuote()
        {
            var samurai = new Samurai { Name = "Ronin" };
            var quote = new Quote { Text = "Aren't I MARVELous?" };
            samurai.Quotes.Add(quote);
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
            quote.Text += " See what I did there?";
            _context.SaveChanges();
        }
    }
}