﻿using System.Collections.Generic;

namespace SamuraiApp.Domain
{
  public class Samurai
  {
    public Samurai()
    {
      Quotes = new List<Quote>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public List<Quote> Quotes { get; set; }
    public int BattleId { get; set; }
    }
}