using CustomerService.Messaging.RabbitMQ.Outbox;
using CustomerService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CustomerService.ModelConfiguration
{

    public class MessageConfig : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Payload).HasMaxLength(8000);
            builder.Property(c => c.Type).HasMaxLength(500);
            builder.Property(c => c.IsProcced).HasDefaultValue(false);

        }
    }
}