﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCoreUWP {
  public class BingeService {
    public static void RecordBinge(int count, bool worthIt) {
      var binge = new CookieBinge {
        HowMany = count,
        WorthIt = worthIt,
        TimeOccurred = DateTime.Now
      };

      using (var context = new BingeContext()) {
        context.Binges.Add(binge);
        context.SaveChanges();
      }
    }

    public static IEnumerable<CookieBinge> GetLast5Binges() {
      using (var context = new BingeContext()) {
        var latestBinges = context.Binges
          .OrderByDescending(b => b.TimeOccurred)
          .Take(5).ToList();

        return latestBinges;
      }
    }

    public static void ClearHistory()
    {
      using (var context = new BingeContext())
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
      }
    }
  }
}