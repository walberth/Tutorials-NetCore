using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void QueryWithFunction()
        {
            var inMemoryOptions = new DbContextOptionsBuilder<SamuraiContext>()
                .UseInMemoryDatabase("QueryWithFunction").Options;
            var context = new SamuraiContext(inMemoryOptions);
            SeedData(context);
            var battles = context.Battles
                .Select(
                b => new {
                    b.Name,
                    Days = SamuraiContext.DaysInBattle(b.StartDate, b.EndDate)
                })
                 .ToList();
            Assert.AreNotEqual(0, battles.Count());
        }

        private static void SeedData(SamuraiContext context)
        {
            var battles = new List<Battle>{
            new Battle
                  {
                Name = "Battle of Okehazama",
                StartDate = new DateTime(1560, 05, 01),
                EndDate = new DateTime(1560, 06, 15)
            },
            new Battle
            {
                Name = "Battle of Shiroyama",
                StartDate = new DateTime(1877, 09, 24),
                EndDate = new DateTime(1877, 09, 24)
            },
            new Battle
            {
                Name = "Siege of Osaka",
                StartDate = new DateTime(1614, 01, 01),
                EndDate = new DateTime(1615,12,31)
            },
             new Battle
            {
                Name = "Boshin War",
                StartDate = new DateTime(1868, 01, 01),
                EndDate = new DateTime(1869,01,01)
            }
            };
            context.Battles.AddRange(battles);
            context.SaveChanges();
        }
    }
}