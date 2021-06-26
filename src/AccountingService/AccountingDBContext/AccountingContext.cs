using AccountingService.ModelConfiguration;
using AccountingService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AccountingService.AccountingDBContext
{
    public class AccountingContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public AccountingContext(IConfiguration configuration, ILogger<AccountingContext> logger)
        {
            _configuration = configuration;
            _logger = logger;

        }
        public DbSet<AccountingCustomer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AccountingCustomerConfig());
            base.OnModelCreating(modelBuilder);
        }
    }
}
