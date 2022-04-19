using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
  public class ConnectedData
  {
    private SamuraiContext _context;

    public ConnectedData() {
      _context = new SamuraiContext();
    }

    public LocalView<Samurai> SamuraisListInMemory() {
      if (_context.Samurais.Local.Count == 0) {
        _context.Samurais.ToList();
      }
      return _context.Samurais.Local;
    }

    public Samurai LoadSamuraiGraph(int samuraiId) {
      var samurai = _context.Samurais.Find(samuraiId); //gets from tracker if its there
      _context.Entry(samurai).Reference(s => s.SecretIdentity).Load();
      _context.Entry(samurai).Collection(s => s.Quotes).Load();

      return samurai;
    }

    public Samurai CreateNewSamurai() {
      var samurai = new Samurai { Name = "New Samurai" };
      _context.Samurais.Add(samurai);
      return samurai;
    }

    public void SaveChanges() {
      _context.SaveChanges();
    }
  }
}