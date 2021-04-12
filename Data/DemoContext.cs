using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class DemoContext:DbContext
    {
        public DemoContext()
        {
            //设置所有的查询语句不进行状态跟踪
            //ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }
        public DbSet<League> Leagues { get; set; }
        public DbSet<Clue> Clues { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Resume> Resumes { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseLoggerFactory(ConsoleLoggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer("Server=.;Database=EFCoreDemo;Trusted_Connection=True;");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GamePlayer>().HasKey(x => new { x.GameId, x.PlayerId });
            modelBuilder.Entity<Resume>().HasOne(x => x.Player).WithOne(x => x.Resume).HasForeignKey<Resume>(x => x.PlayerId);
        }

        public static readonly ILoggerFactory ConsoleLoggerFactory = LoggerFactory.Create(builder => {
            builder.AddFilter((category, lever) => {
                return category == DbLoggerCategory.Database.Command.Name && lever == LogLevel.Information;
            }).AddConsole();
        });
    }
}
