using MaktabeGulabProject.Models;
using Microsoft.EntityFrameworkCore;

namespace MaktabeGulabProject.Data
{
    public class MaktabDbContext : DbContext

    {
        public MaktabDbContext(DbContextOptions<MaktabDbContext> options) : base(options) {
        }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet <Section> Sections { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Result> Results { get; set; }
        public DbSet<Class> Class{ get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Timing> Timings { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
