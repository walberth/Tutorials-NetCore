using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using System;
using System.Linq;

namespace SomeUI
{
    internal class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        private static void Main(string[] args)
        {
            //RetrieveScalarResult();
            //FilterWithScalarResult();
            //SortWithScalar();
            //SortWithoutReturningScalar();
            //RetrieveBattleDays();
            //RetrieveBattleDaysWithoutDbFunction();
     
        }

      
        private static void RetrieveYearUsingDbBuiltInFunction()
        {
           var battles=_context.Battles
                .Select(b=>new { b.Name, b.StartDate.Year }).ToList();
        }

        private static void RetrieveScalarResult()
        {
            var samurais = _context.Samurais
                .Select(s => new
                {
                    s.Name,
                    EarliestBattle = SamuraiContext.EarliestBattleFoughtBySamurai(s.Id)
                })
                .ToList();
        }
        private static void FilterWithScalarResult()
        {
            var samurais = _context.Samurais
                    .Where(s => EF.Functions.Like(SamuraiContext.EarliestBattleFoughtBySamurai(s.Id), "%Battle%"))
                    .Select(s => new
                    {
                        s.Name,
                        EarliestBattle = SamuraiContext.EarliestBattleFoughtBySamurai(s.Id)
                    })
                   .ToList();
        }
        private static void SortWithScalar()
        {
            var samurais = _context.Samurais
                 .OrderBy(s => SamuraiContext.EarliestBattleFoughtBySamurai(s.Id))
                 .Select(s => new { s.Name, EarliestBattle = SamuraiContext.EarliestBattleFoughtBySamurai(s.Id) })
                 .ToList();
        }
        private static void SortWithoutReturningScalar()
        {
            var samurais = _context.Samurais
                 .OrderBy(s => SamuraiContext.EarliestBattleFoughtBySamurai(s.Id))
                 .ToList();
        }
        private static void RetrieveBattleDays()
        {
            var battles = _context.Battles.Select(b => new { b.Name, Days = SamuraiContext.DaysInBattle(b.StartDate, b.EndDate) }).ToList();
        }

        private static void RetrieveBattleDaysWithoutDbFunction()
        {
            var battles = _context.Battles.Select(
                b => new {
                    b.Name,
                    Days = DateDiffDaysPlusOne(b.StartDate, b.EndDate)
                }
                ).ToList();
        }
        private static int DateDiffDaysPlusOne(DateTime start, DateTime end)
        {
            return (int)end.Subtract(start).TotalDays + 1;
        }

    }
}