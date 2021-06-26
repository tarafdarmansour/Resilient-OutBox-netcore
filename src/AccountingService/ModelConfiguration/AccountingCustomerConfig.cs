using AccountingService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccountingService.ModelConfiguration
{

    public class AccountingCustomerConfig : IEntityTypeConfiguration<AccountingCustomer>
    {
        public void Configure(EntityTypeBuilder<AccountingCustomer> builder)
        {
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.FirstName).HasMaxLength(200);
            builder.Property(c => c.LastName).HasMaxLength(200);
            builder.Property(c => c.CreateDateTime).HasDefaultValueSql("GETDATE()");

        }
    }
}