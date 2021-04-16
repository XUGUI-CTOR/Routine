using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EFCoreCode_First
{
    public class BloggingContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Person> Persons { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:\\blogging.db").LogTo(Console.WriteLine, LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().Property(p => p.DisplayName).HasComputedColumnSql("[LastName]+ ' '+ [FirstName]");
        }
    }
    public class Blog
    {
        public Blog()
        {
            Posts = new List<Post>();
        }
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "varchar(150)"),Required]
        public string Url { get; set; }
        public ICollection<Post> Posts { get; set; }
        [NotMapped]
        public DateTime LoadedFromDatabase { get; set; }

        [Column(TypeName = "decimal(5, 2)")]
        public decimal Rating { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        [Column(TypeName = "varchar(80)")]
        public string Title { get; set; }
        [Column(TypeName = "varchar(200)")]
        public string Content { get; set; }
        [Column("blog_id")]
        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }

    public class Person
    {
        [Key]
        public int PersonId { get; set; }
        [Column(TypeName = "varchar(30)")]
        public string FirstName { get; set; }
        [Column(TypeName = "varchar(28)")]
        public string LastName { get; set; }
        public string DisplayName { get; set; }
    }
}
