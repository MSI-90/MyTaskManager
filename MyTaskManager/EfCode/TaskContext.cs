﻿using Models.EfClasses;
using Microsoft.EntityFrameworkCore;


namespace MyTaskManager.EfCode
{
    public class TaskContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public DbSet<MyTask> Tasks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }

        public TaskContext(DbContextOptions<TaskContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseNpgsql(_configuration.GetConnectionString("defaultConnection"));
    }
}
