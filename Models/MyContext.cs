using Microsoft.EntityFrameworkCore;
namespace BeltExam.Models
{
    public class MyContext : DbContext
    {
        public MyContext (DbContextOptions options) : base(options) { }

        public DbSet<User> Users {get;set;}
        public DbSet<Exercise> Exercises {get;set;}
        public DbSet<Association> Associations {get;set;}
    }
}