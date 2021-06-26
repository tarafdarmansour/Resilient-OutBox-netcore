using System.Threading;
using System.Threading.Tasks;
using AccountingService.AccountingDBContext;
using AccountingService.Models;
using CustomerService.Events;
using MediatR;

namespace DashboardService.Listeners
{
    public class CustomerDeletedHandler : INotificationHandler<CustomerDeleted>
    {
        private readonly AccountingContext _context;

        public CustomerDeletedHandler(AccountingContext context)
        {
            _context = context;
        }

        public async Task Handle(CustomerDeleted notification, CancellationToken cancellationToken)
        {

            var customer = await _context.Customers.FindAsync(notification.Id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);

                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
