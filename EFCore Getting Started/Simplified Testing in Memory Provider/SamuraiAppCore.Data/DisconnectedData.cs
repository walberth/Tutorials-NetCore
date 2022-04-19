using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SamuraiAppCore.Domain;

namespace SamuraiAppCore.Data
{
  public class DisconnectedData
  {
    private SamuraiContext _context;

    public DisconnectedData(SamuraiContext context) {
      _context = context;
      _context.ChangeTracker.QueryTrackingBehavior
        = QueryTrackingBehavior.NoTracking;
    }

    public List<KeyValuePair<int, string>> GetSamuraiReferenceList() {
      var samurais = _context.Samurais.OrderBy(s => s.Name)
        .Select(s => new {s.Id, s.Name})
        .ToDictionary(t => t.Id, t => t.Name).ToList();
      return samurais;
    }

  
    public Samurai LoadSamuraiGraph(int id) {
      var samurai =
        _context.Samurais
        .Include(s => s.SecretIdentity)
        .Include(s => s.Quotes)
        .FirstOrDefault(s => s.Id == id);
     return samurai;
    }


    public void SaveSamuraiGraph(Samurai samurai)
    {
      _context.ChangeTracker.TrackGraph
        (samurai, e=>ApplyStateUsingIsKeySet(e.Entry));
      _context.SaveChanges();
    }

    private static void ApplyStateUsingIsKeySet(EntityEntry entry) {
      if (entry.IsKeySet) {
        if (((ClientChangeTracker) entry.Entity).IsDirty) {
          entry.State = EntityState.Modified;
        }
        else {
          entry.State = EntityState.Unchanged;
        }
      }
      else {
        entry.State = EntityState.Added;
      }
    }

    public void DeleteSamuraiGraph(int id)
    {
      //goal:  delete samurai , quotes and secret identity
      //       also delete any joins with battles
      //EF Core supports Cascade delete by convention
      //Even if full graph is not in memory, db is defined to delete
      //But always double check!
      var samurai = _context.Samurais.Find(id); //NOT TRACKING !!
      _context.Entry(samurai).State=EntityState.Deleted; //TRACKING
      _context.SaveChanges();



    }
  }
}