using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SomeUI
{
  internal static class Module4Methods
  {

   private static SamuraiContext _context = new SamuraiContext();

    public static void  RunAll()
    {
     
    // InsertSamurai();
    //InsertMultipleSamurais();
      AddSomeMoreSamurais();
    //SimpleSamuraiQuery();
    //MoreQueries();
    //RetrieveAndUpdateSamurai();
    //RetrieveAndUpdateMultipleSamurais();
    //MultipleOperations();
    //QueryAndUpdateSamuraiDisconnected();
    //QueryAndUpdateDisconnectedBattle();
    //RawSqlQuery();
    //RawSqlCommand();
    //RawSqlCommandWithOutput();
    //AddSomeMoreSamurais();
    //DeleteWhileTracked();
    //DeleteWhileNotTracked();
    // DeleteMany();
  }

    private static void AddSomeMoreSamurais() {
      _context.AddRange(
        new Samurai { Name = "Kambei Shimada" },
        new Samurai { Name = "Shichirōji ", IsDirty = true},
        new Samurai { Name = "Katsushirō Okamoto" },
        new Samurai { Name = "Heihachi Hayashida" },
        new Samurai { Name = "Kyūzō" },
        new Samurai { Name = "Gorōbei Katayama" }
        );
      _context.SaveChanges();
    }

    private static void DeleteWhileTracked() {
      var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Kambei Shimada");
      _context.Samurais.Remove(samurai);
      //alternates:
      // _context.Remove(samurai);
      // _context.Entry(samurai).State=EntityState.Deleted;
      // _context.Samurais.Remove(_context.Samurais.Find(1));
      _context.SaveChanges();
    }

    private static void DeleteMany() {
      var samurais = _context.Samurais.Where(s => s.Name.Contains("ō"));
      _context.Samurais.RemoveRange(samurais);
      //alternate: _context.RemoveRange(samurais);
      _context.SaveChanges();
    }

    private static void DeleteWhileNotTracked() {
      var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Heihachi Hayashida");
      using (var contextNewAppInstance = new SamuraiContext()) {
        contextNewAppInstance.Samurais.Remove(samurai);
        //contextNewAppInstance.Entry(samurai).State=EntityState.Deleted;
        contextNewAppInstance.SaveChanges();
      }

    }


    private static void RawSqlQuery() {
      //var samurais= _context.Samurais.FromSql("Select * from Samurais")
      //              .OrderByDescending(s => s.Name)
      //              .Where(s=>s.Name.Contains("San")).ToList();
      var namePart = "San";
      var samurais = _context.Samurais
        .FromSql("EXEC FilterSamuraiByNamePart {0}", namePart)
        .OrderByDescending(s => s.Name).ToList();

      samurais.ForEach(s => Console.WriteLine(s.Name));
      Console.WriteLine();
    }


    private static void RawSqlCommand() {
      var affected = _context.Database.ExecuteSqlCommand(
        "update samurais set Name=REPLACE(Name,'San','Nan')");
      Console.WriteLine($"Affected rows {affected}");
    }


    private static void RawSqlCommandWithOutput() {
      var procResult = new SqlParameter
      {
        ParameterName = "@procResult",
        SqlDbType = SqlDbType.VarChar,
        Direction = ParameterDirection.Output,
        Size = 50
      };
      _context.Database.ExecuteSqlCommand(
        "exec FindLongestName @procResult OUT", procResult);
      Console.WriteLine($"Longest name: {procResult.Value}");
    }


    private static void QueryWithNonSql() {
      var samurais = _context.Samurais
        .Select(s => new { newName = ReverseString(s.Name) })
        .ToList();
      samurais.ForEach(s => Console.WriteLine(s.newName));
      Console.WriteLine();
    }

    private static string ReverseString(string value) {
      var stringChar = value.AsEnumerable();
      return String.Concat(stringChar.Reverse());
    }

    private static void QueryAndUpdateDisconnectedBattle() {
      var battle = _context.Battles.FirstOrDefault();
      battle.EndDate = new DateTime(1754, 12, 31);
      using (var contextNewAppInstance = new SamuraiContext()) {
        contextNewAppInstance.Battles.Update(battle);
        contextNewAppInstance.SaveChanges();
      }
    }

    private static void QueryAndUpdateSamuraiDisconnected() {
      var samurai = _context.Samurais.FirstOrDefault(s => s.Name == "Kikuchiyo");
      samurai.Name += "San";
      using (var contextNewAppInstance = new SamuraiContext()) {
        contextNewAppInstance.Samurais.Update(samurai);
        contextNewAppInstance.SaveChanges();
      }

    }

    private static void MultipleOperations() {
      var samurai = _context.Samurais.FirstOrDefault();
      samurai.Name += "San";
      _context.Samurais.Add(new Samurai { Name = "Kikuchiyo" });
      _context.SaveChanges();
    }

    private static void RetrieveAndUpdateMultipleSamurais() {
      var samurais = _context.Samurais.ToList();
      samurais.ForEach(s => s.Name += "San");
      _context.SaveChanges();
    }

    private static void RetrieveAndUpdateSamurai() {
      var samurai = _context.Samurais.FirstOrDefault();
      samurai.Name += "San";
      _context.SaveChanges();
    }


    private static void InsertSamurai() {
      var samurai = new Samurai { Name = "Julie" };
      using (var context = new SamuraiContext()) {
        context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
        context.Samurais.Add(samurai);
        context.SaveChanges();
      }
    }

    private static void InsertMultipleSamurais() {
      var samurai = new Samurai { Name = "Julie" };
      var samuraiSammy = new Samurai { Name = "Sampson" };
      using (var context = new SamuraiContext()) {
        context.GetService<ILoggerFactory>().AddProvider(new MyLoggerProvider());
        context.Samurais.AddRange(new List<Samurai> { samurai, samuraiSammy });

        context.SaveChanges();
      }
    }

    private static void SimpleSamuraiQuery() {
      using (var context = new SamuraiContext()) {
        var samurais = context.Samurais.ToList();
        // var query = context.Samurais();
        foreach (var samurai in samurais) {
          Console.Write(samurai.Name);
        }
      }
    }

    private static void MoreQueries() {
      //var name = "Julie";
      var samurais = _context.Samurais.Find(2);
    }
  }
}
