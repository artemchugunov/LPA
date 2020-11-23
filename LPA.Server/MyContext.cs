using Microsoft.EntityFrameworkCore;

using LPA.Server.Models;

namespace LPA.Server
{
    public class MyContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<StudentCourse> StudentCourses { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<TestQuestion> TestQuestions { get; set; }
        public DbSet<TestQuestionAnswer> TestQuestionAnswers { get; set; }
        public DbSet<StudentTestSuccess> StudentTestSuccesses { get; set; }














        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=ec2-46-137-156-205.eu-west-1.compute.amazonaws.com;Port=5432;Database=;User Id=;Password=;SSL MODE=Require;Trust Server Certificate=true");
        }
    }
}