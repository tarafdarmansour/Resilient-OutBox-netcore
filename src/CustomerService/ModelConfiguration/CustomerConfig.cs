using CustomerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.ModelConfiguration
{

    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.FirstName).HasMaxLength(200);
            builder.Property(c => c.LastName).HasMaxLength(200);
            builder.Property(c => c.CreateDateTime).HasDefaultValueSql("GETDATE()");

        }
    }
}