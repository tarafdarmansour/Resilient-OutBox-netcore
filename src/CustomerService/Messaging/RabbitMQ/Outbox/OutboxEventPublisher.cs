using CustomerService.CustomerDBContext;
using System.Threading.Tasks;

namespace CustomerService.Messaging.RabbitMQ.Outbox
{
    public class OutboxEventPublisher : IEventPublisher
    {
        private readonly CustomerContext _context;

        public OutboxEventPublisher(CustomerContext context)
        {
            _context = context;
        }

        public async Task PublishMessage<T>(T msg)
        {
            await _context.Messages.AddAsync(new Message(msg));
        }
    }
}