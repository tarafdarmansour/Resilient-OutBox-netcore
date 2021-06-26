using CustomerService.Messaging.RabbitMQ.Outbox;
using CustomerService.ModelConfiguration;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace CustomerService.CustomerDBContext
{
    public class CustomerContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public CustomerContext(IConfiguration configuration, ILogger<CustomerContext> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Message> Messages { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerConfig());
            modelBuilder.ApplyConfiguration(new MessageConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
