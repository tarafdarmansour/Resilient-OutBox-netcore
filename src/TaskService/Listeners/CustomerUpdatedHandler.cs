using System.Threading;
using System.Threading.Tasks;
using TaskService.TaskDBContext;
using TaskService.Models;
using CustomerService.Events;
using MediatR;

namespace DashboardService.Listeners
{
    public class CustomerUpdatedHandler : INotificationHandler<CustomerUpdated>
    {
        private readonly TaskContext _context;

        public CustomerUpdatedHandler(TaskContext context)
        {
            _context = context;
        }

        public async Task Handle(CustomerUpdated notification, CancellationToken cancellationToken)
        {

            var customer = await _context.Customers.FindAsync(notification.Id);
            if (customer != null)
            {
                customer.UpdateTaskCustomer(notification);
                _context.Customers.Update(customer);

                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
