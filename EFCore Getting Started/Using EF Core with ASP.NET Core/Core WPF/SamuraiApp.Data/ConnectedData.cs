﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
  public class ConnectedData
  {
    private SamuraiContext _context;

    public ConnectedData() {
      _context = new SamuraiContext();
      _context.Database.EnsureCreated();
    }

    public Samurai CreateNewSamurai() {
      //battles (many to many) will not be involved
      var samurai = new Samurai { Name = "New Samurai" };
      _context.Samurais.Add(samurai);
      return samurai;
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

    public void SaveChanges(Type typeBeingEdited)
    {
      _context.SaveChanges();
      if (typeBeingEdited == typeof (Samurai)) { 
        if (_context.Samurais.Local.Any()) {
          SamuraisListInMemory().ToList().ForEach(s => s.IsDirty = false);
        }
      }
      if (typeBeingEdited == typeof (Battle)) {
        if (_context.Battles.Local.Any()) {
          BattlesListInMemory().ToList().ForEach(s => s.IsDirty = false);
        }
      }

    }

    public LocalView<Battle> BattlesListInMemory() {
      if (_context.Battles.Local.Count == 0) {
        _context.Battles.ToList();
      }
      return _context.Battles.Local;
    }

    public List<Samurai> SamuraisNotInBattle(int battleId)
    {
      var existingSamurais = _context.SamuraiBattles
        .Where(sb => sb.BattleId == battleId)
        .Select(sb => sb.SamuraiId).ToList();
      var samurais = _context.Samurais.AsNoTracking() 
        .Where(s => !existingSamurais.Contains(s.Id))
        .ToList();

      return samurais;
    } 

    public Battle LoadBattleGraph(int battleId) {
      var battle = _context.Battles.Find(battleId); //gets from tracker if its there
   
       _context.Entry(battle).Collection(b=>b.SamuraiBattles).Query().Include(sb=>sb.Samurai).Load();
      return battle;
    }

    public void AddSamuraiBattle(SamuraiBattle samuraiBattle) {
      //presumes samurai and battle always already exist
     _context.Entry(samuraiBattle).State
        =EntityState.Added;
    }

   

    public void RevertBattleChanges(int id)
    {
      //this is the simplest way. 
      //Maybe later versions of EF will make it easier
     _context=new SamuraiContext();
    }

    public Battle CreateNewBattle()
    {
      //samurais (many to many) will not be involved
      var battle = new Battle{ Name = "New Battle" };
      _context.Battles.Add(battle);
      return battle;
    }
  }
}