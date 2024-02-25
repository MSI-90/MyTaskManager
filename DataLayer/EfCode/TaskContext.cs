using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MyTaskManager.Data;

namespace DataLayer.EfCode
{
    public class TaskContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<MyTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Priority> Priority { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("defaultConnection"));
    }
}
