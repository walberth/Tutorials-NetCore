using Microsoft.EntityFrameworkCore;
using SamuraiApp.Data;
using SamuraiApp.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SomeUI
{
    class Program
    {
        private static SamuraiContext _context = new SamuraiContext();

        static void Main(string[] args)
        {
            //PrePopulateSamuraisAndBattles();
            //JoinBattleAndSamurai();
            //EnlistSamuraiIntoABattle();
            //EnlistSamuraiIntoABattleUntracked();
            //AddNewSamuraiViaDisconnectedBattleObject();
            //GetSamuraiWithBattles();
            //GetBattlesForSamuraiInMemory();
            //RemoveJoinBetweenSamuraiAndBattleSimple();
            //RemoveBattleFromSamurai();
            //RemoveBattleFromSamuraiWhenDisconnected();
            //AddNewSamuraiWithSecretIdentity();
            //AddSecretIdentityUsingSamuraiId();
            //AddSecretIdentityToExistingSamurai();
            //EditASecretIdentity();
            // ReplaceASecretIdentity();
            //ReplaceASecretIdentityNotTracked();
            //ReplaceSecretIdentityNotInMemory();
          
           }

        private static void ReplaceSecretIdentityNotInMemory()
        {
            var samurai = _context.Samurais.FirstOrDefault(s => s.SecretIdentity != null);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Bobbie Draper" };
            _context.SaveChanges(); 
        } 
        private static void ReplaceASecretIdentityNotTracked()
        {
            Samurai samurai;
            using (var separateOperation = new SamuraiContext())
            {
                samurai = separateOperation.Samurais.Include(s => s.SecretIdentity)
                                           .FirstOrDefault(s => s.Id == 1);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
            _context.Samurais.Attach(samurai);
            //this will fail...EF Core tries to insert a duplicate samuraiID FK
            _context.SaveChanges();
        }
        private static void ReplaceASecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                                  .FirstOrDefault(s => s.Id == 1);
            samurai.SecretIdentity = new SecretIdentity { RealName = "Sampson" };
            _context.SaveChanges();
        }
        private static void EditASecretIdentity()
        {
            var samurai = _context.Samurais.Include(s => s.SecretIdentity)
                                  .FirstOrDefault(s => s.Id == 1);
            samurai.SecretIdentity.RealName = "T'Challa";
            _context.SaveChanges();
        }
        private static void AddSecretIdentityToExistingSamurai()
        {
            Samurai samurai;
            using (var separateOperation = new SamuraiContext())
            {
                samurai = _context.Samurais.Find(2);
            }
            samurai.SecretIdentity = new SecretIdentity { RealName = "Julia" };
            _context.Samurais.Attach(samurai);
            _context.SaveChanges();
        }
        private static void AddSecretIdentityUsingSamuraiId()
        {
            //Note: SamuraiId 1 does not have a secret identity yet!
            var identity = new SecretIdentity { SamuraiId = 1,  };
            _context.Add(identity);
            _context.SaveChanges();
        }   
        private static void AddNewSamuraiWithSecretIdentity()
        {
            var samurai = new Samurai { Name = "Jina Ujichika" };
            samurai.SecretIdentity = new SecretIdentity { RealName = "Julie" };
            _context.Samurais.Add(samurai);
            _context.SaveChanges();
        }

        private static void RemoveBattleFromSamuraiWhenDisconnected()
        {
            //Goal:Remove join between Shichirōji(Id=3) and Battle of Okehazama (Id=1)
            Samurai samurai;
            using (var separateOperation = new SamuraiContext())
            {
                samurai = separateOperation.Samurais.Include(s => s.SamuraiBattles)
                                                    .ThenInclude(sb => sb.Battle)
                                           .SingleOrDefault(s => s.Id == 3);
            }
            var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 1);
            samurai.SamuraiBattles.Remove(sbToRemove);
            //_context.Attach(samurai);
            //_context.ChangeTracker.DetectChanges();
            _context.Remove(sbToRemove);
            _context.SaveChanges();
        }
        private static void RemoveBattleFromSamurai()
        {
            //Goal:Remove join between Shichirōji(Id=3) and Battle of Okehazama (Id=1)
            var samurai = _context.Samurais.Include(s => s.SamuraiBattles)
                                           .ThenInclude(sb => sb.Battle)
                                  .SingleOrDefault(s => s.Id == 3);
             var sbToRemove = samurai.SamuraiBattles.SingleOrDefault(sb => sb.BattleId == 1);
             samurai.SamuraiBattles.Remove(sbToRemove); //remove via List<T>
             //_context.Remove(sbToRemove); //remove using DbContext
             _context.ChangeTracker.DetectChanges(); //here for debugging
             _context.SaveChanges();
        }
        private static void RemoveJoinBetweenSamuraiAndBattleSimple()
        {
            var join = new SamuraiBattle { BattleId = 1, SamuraiId = 8 };
            _context.Remove(join);
            _context.SaveChanges();
        }
        private static void GetBattlesForSamuraiInMemory()
        {
            var battle = _context.Battles.Find(1);
            _context.Entry(battle).Collection(b => b.SamuraiBattles).Query().Include(sb => sb.Samurai).Load();
            
        }
        private static void GetSamuraiWithBattles()
        {
            var samuraiWithBattles = _context.Samurais
                .Include(s => s.SamuraiBattles)
                .ThenInclude(sb => sb.Battle).FirstOrDefault(s => s.Id == 1);
            var battle = samuraiWithBattles.SamuraiBattles.First().Battle;
            var allTheBattles = new List<Battle>();
            foreach(var samuraiBattle in samuraiWithBattles.SamuraiBattles)
            {
                allTheBattles.Add(samuraiBattle.Battle);
            }
        }
        private static void AddNewSamuraiViaDisconnectedBattleObject()
        {
            Battle battle;
            using (var separateOperation = new SamuraiContext())
            {
                battle = separateOperation.Battles.Find(1) ;
            }
            var newSamurai = new Samurai { Name = "SampsonSan" };
            battle.SamuraiBattles.Add(new SamuraiBattle {Samurai = newSamurai});
            _context.Battles.Attach(battle);
            _context.ChangeTracker.DetectChanges();
            _context.SaveChanges();
        }

        private static void EnlistSamuraiIntoABattleUntracked()
        {
            Battle battle;
            using (var separateOperation = new SamuraiContext())
            {
                battle = separateOperation.Battles.Find(1) ;
            }
            battle.SamuraiBattles.Add(new SamuraiBattle { SamuraiId = 2 });
            _context.Battles.Attach(battle);
            _context.ChangeTracker.DetectChanges(); //here to show you debugging info
            _context.SaveChanges();

        }
        private static void EnlistSamuraiIntoABattle()
        {
            var battle = _context.Battles.Find(1);
            battle.SamuraiBattles
                .Add(new SamuraiBattle {SamuraiId = 3 });
            _context.SaveChanges();
        }
       
        private static void JoinBattleAndSamurai()
        {
            //Kikuchiyo id is 1, Siege of Osaka id is 3
            var sbJoin = new SamuraiBattle { SamuraiId = 1, BattleId = 3 };
            _context.Add(sbJoin);
            _context.SaveChanges();
        }

        private static void PrePopulateSamuraisAndBattles()
        {
            _context.AddRange(
             new Samurai { Name = "Kikuchiyo" },
             new Samurai { Name = "Kambei Shimada" },
             new Samurai { Name = "Shichirōji " },
             new Samurai { Name = "Katsushirō Okamoto" },
             new Samurai { Name = "Heihachi Hayashida" },
             new Samurai { Name = "Kyūzō" },
             new Samurai { Name = "Gorōbei Katayama" }
           );

            _context.Battles.AddRange(
             new Battle { Name = "Battle of Okehazama", StartDate = new DateTime(1560, 05, 01), EndDate = new DateTime(1560, 06, 15) },
             new Battle { Name = "Battle of Shiroyama", StartDate = new DateTime(1877, 9, 24), EndDate = new DateTime(1877, 9, 24) },
             new Battle { Name = "Siege of Osaka", StartDate = new DateTime(1614, 1, 1), EndDate = new DateTime(1615, 12, 31) },
             new Battle { Name = "Boshin War", StartDate = new DateTime(1868, 1, 1), EndDate = new DateTime(1869, 1, 1) }
           );
           _context.SaveChanges();
        }
    }
}

