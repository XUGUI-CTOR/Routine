using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace EFCoreCode_First
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BloggingContext())
            {
                Console.WriteLine("Inserting a new blog");
                context.Add(new Blog { Url = "http://blogs.msdn.com/adonet" });
                context.SaveChanges();
                Console.WriteLine("querying for a blog");
                var blog = context.Blogs.OrderBy(x => x.Id).FirstOrDefault();
                Console.WriteLine("Updating the blog and adding a post");
                blog.Url = "https://devblogs.microsoft.com/dotnet";
              
                context.SaveChanges();
                
            }
        }


    }

    public class Student
    {
        public int StudentId { get; set; }
        public string Name { get; set; }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
    }

    public class SchoolContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=SchoolDB;Trusted_Connection=True;");
            }
        }
    }
}
