using System;
using System.Collections.Generic;

namespace SamuraiAppCore.Domain
{
  public class Battle:ClientChangeTracker
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<SamuraiBattle> SamuraiBattles { get; set; }
   
  }
}