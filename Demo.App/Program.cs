using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace Demo.App
{
    class Program
    {
        static void Main(string[] args)
        {
            using var context = new DemoContext();
            //var vlubs = context.Clues
            //    .Include(x => x.League)
            //    .Include(x => x.Players)
            //        .ThenInclude(y => y.Resume)
            //    .Include(x=>x.Players)
            //        .ThenInclude(y=>y.GamePlayers)
            //            .ThenInclude(z=>z.Game)
            //    .ToList();
            var Clue = context.Clues
                .Where(x => x.Id == 2).FirstOrDefault();
            context.Entry(Clue).Collection(x => x.Players).Load();
            context.Entry(Clue).Reference(x => x.League).Load();
            Console.WriteLine(Clue.Players?.Count??0);
        }
    }
}
