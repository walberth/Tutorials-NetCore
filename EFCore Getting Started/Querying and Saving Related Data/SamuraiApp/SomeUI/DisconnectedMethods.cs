using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SamuraiApp.Data;
using SamuraiApp.Domain;

namespace SomeUI
{
  internal static class DisconnectedMethods
  {

    private static void DisplayState(List<EntityEntry> es, string method) {
      Console.WriteLine(method);
      es.ForEach(e => Console.WriteLine(
        $"{e.Entity.GetType().Name} : {e.State.ToString()}"));
      Console.WriteLine();
    }

    public static void AddGraphAllNew()
    {
      var samuraiGraph = new Samurai {Name = "Julie"};
      samuraiGraph.Quotes.Add(new Quote {Text="This is new"});
      using (var context = new SamuraiContext()) {
        context.Samurais.Add(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AddGraphAllNew");
      }
    }
    public static void AddGraphWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie",Id=1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is not new", Id=1 });
      using (var context = new SamuraiContext()) {
        context.Samurais.Add(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AddGraphWithKeyValues");
      }
    }

    public static void AttachGraphAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new"});
      using (var context = new SamuraiContext()) {
        context.Samurais.Attach(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
      DisplayState(es, "AttachGraphAllNew");
      }
    }
    public static void AttachGraphWithKeys() {
      var samuraiGraph = new Samurai { Name = "Julie",Id=1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is not new",Id=1 });
      using (var context = new SamuraiContext()) {
        context.Samurais.Attach(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AttachGraphWithKeys");
      }
    }

    public static void UpdateGraphAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.Samurais.Update(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "UpdateGraphAllNew");
      }
    }
    public static void UpdateGraphWithKeys() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is edited", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.Samurais.Update(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "UpdateGraphWithKeys");
      }
    }

    public static void DeleteGraphAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.Samurais.Remove(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "DeleteGraphAllNew");
        context.SaveChanges();
      }
    }
    public static void DeleteGraphWithKeys() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is not new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.Samurais.Remove(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "DeleteGraphWithKeys");
      }
    }
    public static void VerifyingThatOrphanedNewChildDoesntGetSentToDatabase() {
      var samuraiGraph = new Samurai { Name = "Julie" };
        using (var context = new SamuraiContext())
      {
        context.Samurais.Add(samuraiGraph);
        context.SaveChanges();
        samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
        context.Samurais.Remove(samuraiGraph);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "DeleteGraphAllNew");
        context.SaveChanges();
      }
    }


    public static void AddGraphViaEntryAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.Entry(samuraiGraph).State = EntityState.Added;
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AddGraphViaEntryAllNew");
      }
    }
    public static void AddGraphViaEntryWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.Entry(samuraiGraph).State = EntityState.Added;
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AddGraphViaEntryWithKeyValues");
      }
    }

    public static void AttachGraphViaEntryAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.Entry(samuraiGraph).State = EntityState.Unchanged;
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AttachGraphViaEntryAllNew");
      }
    }
    public static void AttachGraphViaEntryWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.Entry(samuraiGraph).State = EntityState.Unchanged;
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AttachGraphViaEntryWithKeyValues");
      }
    }
   
    public static void UpdateGraphViaEntryWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.Entry(samuraiGraph).State = EntityState.Modified;
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "UpdateGraphViaEntryWithKeyValues");
      }
    }
    public static void UpdateGraphViaEntryAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.Entry(samuraiGraph).State = EntityState.Modified;
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "UpdateGraphViaEntryAllNew");
      }
    }

    public static void DeleteGraphViaEntryWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.Entry(samuraiGraph).State = EntityState.Deleted;
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "DeleteGraphViaEntryWithKeyValues");
      }
    }
    public static void DeleteGraphViaEntryAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.Entry(samuraiGraph).State = EntityState.Deleted;
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "DeleteGraphViaEntryAllNew");
      }
    }


    public static void ChangeStateUsingEntry()
    {
      var samurai = new Samurai { Name = "She Who Changes State", Id = 1 };
      using (var context = new SamuraiContext()) {
        context.Entry(samurai).State = EntityState.Modified;
        Console.WriteLine("Change State Using Entry");
        DisplayState(context.ChangeTracker.Entries().ToList(), "Initial State");

        context.Entry(samurai).State = EntityState.Added;
        DisplayState(context.ChangeTracker.Entries().ToList(), "New State");
        //context.SaveChanges(); //this will throw a db error if id is db generated

      }
    }

    public static void AddGraphViaTrackGraphWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
          (samuraiGraph,e=>e.Entry.State=EntityState.Added);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AddGraphViaTrackGraphWithKeyValues");
      }
    }
    public static void AddGraphViaTrackGraphAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
          (samuraiGraph, e => e.Entry.State = EntityState.Added);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AddGraphViaTrackGraphAllNew");
      }
    }

    public static void AttachGraphViaTrackGraphWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
           (samuraiGraph, e => e.Entry.State = EntityState.Unchanged);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AttachGraphViaTrackGraphWithKeyValues");
      }
    }
    public static void AttachGraphViaTrackGraphAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
          (samuraiGraph, e => e.Entry.State = EntityState.Unchanged);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "AttachGraphViaTrackGraphAllNew");
      }
    }

    public static void UpdateGraphViaTrackGraphWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
          (samuraiGraph, e => e.Entry.State = EntityState.Modified);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "UpdateGraphViaTrackGraphWithKeyValues");
      }
    }
    public static void UpdateGraphViaTrackGraphAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
            (samuraiGraph, e => e.Entry.State = EntityState.Modified);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "UpdateGraphViaTrackGraphAllNew");
      }
    }

    public static void DeleteGraphViaTrackGraphWithKeyValues() {
      var samuraiGraph = new Samurai { Name = "Julie", Id = 1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new", Id = 1 });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
           (samuraiGraph, e => e.Entry.State = EntityState.Deleted);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "DeleteGraphViaTrackGraphWithKeyValues");
      }
    }
    public static void DeleteGraphViaTrackGraphAllNew() {
      var samuraiGraph = new Samurai { Name = "Julie" };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
           (samuraiGraph, e => e.Entry.State = EntityState.Deleted);
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "DeleteGraphViaTrackGraphAllNew");
      }
    }


    public static void StartTrackingUsingCustomFunction() {
      var samuraiGraph = new Samurai { Name = "Julie",Id=1 };
      samuraiGraph.Quotes.Add(new Quote { Text = "This is new" });
      using (var context = new SamuraiContext()) {
        context.ChangeTracker.TrackGraph
           (samuraiGraph, e => ApplyStateUsingIsKeySet(e.Entry));
        var es = context.ChangeTracker.Entries().ToList();
        DisplayState(es, "StartTrackingUsingCustomFunction");
      }
    }
    private static void ApplyStateUsingIsKeySet(EntityEntry entry) {
      if (entry.IsKeySet) {
        entry.State = EntityState.Unchanged;
      }
      else {
        entry.State = EntityState.Added;
      }
    }

  }
}
