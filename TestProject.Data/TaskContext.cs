using Microsoft.EntityFrameworkCore;

namespace TestProject.Data
{
    public class TaskContext : DbContext
    {
        public TaskContext() { }
        public TaskContext(DbContextOptions<TaskContext> options) : base(options) { }
        public DbSet<TaskTable> TaskTable { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TaskTable>().ToTable("tasktable");
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("DefaultConnection"));
            }
        }
    }
}