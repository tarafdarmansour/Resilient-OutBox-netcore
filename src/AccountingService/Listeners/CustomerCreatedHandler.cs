using System.Threading;
using System.Threading.Tasks;
using AccountingService.AccountingDBContext;
using AccountingService.Models;
using CustomerService.Events;
using MediatR;

namespace DashboardService.Listeners
{
    public class CustomerCreatedHandler : INotificationHandler<CustomerCreated>
    {
        private readonly AccountingContext _context;

        public CustomerCreatedHandler(AccountingContext context)
        {
            _context = context;
        }

        public async Task Handle(CustomerCreated notification, CancellationToken cancellationToken)
        {

            var customer = await _context.Customers.FindAsync(notification.Id);
            if (customer == null)
            {
                var accountingCustomer = new AccountingCustomer(notification);
                _context.Customers.Add(accountingCustomer);

                await _context.SaveChangesAsync(cancellationToken);
            }

        }
    }
}
