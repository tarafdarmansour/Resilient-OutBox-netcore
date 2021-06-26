using TaskService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TaskService.ModelConfiguration
{

    public class TaskCustomerConfig : IEntityTypeConfiguration<TaskCustomer>
    {
        public void Configure(EntityTypeBuilder<TaskCustomer> builder)
        {
            builder.Property(c => c.Id).ValueGeneratedNever();
            builder.Property(c => c.FirstName).HasMaxLength(200);
            builder.Property(c => c.LastName).HasMaxLength(200);
            builder.Property(c => c.CreateDateTime).HasDefaultValueSql("GETDATE()");

        }
    }
}