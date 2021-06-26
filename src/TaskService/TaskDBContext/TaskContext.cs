using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaskService.ModelConfiguration;
using TaskService.Models;

namespace TaskService.TaskDBContext
{
    public class TaskContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public TaskContext(IConfiguration configuration, ILogger<TaskContext> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }
        public DbSet<TaskCustomer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TaskCustomerConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
