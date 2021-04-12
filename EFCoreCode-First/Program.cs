using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
namespace EFCoreCode_First
{
    class Program
    {
        static void Main(string[] args)
        {
            using var dbcontext = new SchoolContext();
            var getname = new Func<string>(() => "warboss");
            var student = dbcontext.Students.Where(x => x.Name == getname()).FirstOrDefault();
            Console.WriteLine($"{student.Name} by {student.StudentId}");
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
