using System.Threading;
using System.Threading.Tasks;
using TaskService.TaskDBContext;
using TaskService.Models;
using CustomerService.Events;
using MediatR;

namespace DashboardService.Listeners
{
    public class CustomerCreatedHandler : INotificationHandler<CustomerCreated>
    {
        private readonly TaskContext _context;

        public CustomerCreatedHandler(TaskContext context)
        {
            _context = context;
        }

        public async Task Handle(CustomerCreated notification, CancellationToken cancellationToken)
        {

            var customer = await _context.Customers.FindAsync(notification.Id);
            if (customer == null)
            {
                var accountingCustomer = new TaskCustomer(notification);
                _context.Customers.Add(accountingCustomer);

                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
