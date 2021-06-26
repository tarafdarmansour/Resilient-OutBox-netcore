using CustomerService.CustomerDBContext;
using CustomerService.Events;
using CustomerService.Messaging;
using CustomerService.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerService.Command
{
    public class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, DeleteCustomerResult>
    {
        private readonly ILogger<DeleteCustomerHandler> _logger;
        private readonly CustomerContext _context;
        private readonly IEventPublisher _eventPublisher;

        public DeleteCustomerHandler(ILogger<DeleteCustomerHandler> logger, CustomerContext context, IEventPublisher eventPublisher)
        {
            _logger = logger;
            _context = context;
            _eventPublisher = eventPublisher;
        }
        public async Task<DeleteCustomerResult> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Customers.FindAsync(request.Id);

            if (entity != null)
            {
                _context.Customers.Remove(entity);

                await _eventPublisher.PublishMessage(CustomerDeleted(entity));

                await _context.SaveChangesAsync(cancellationToken);

                return new DeleteCustomerResult { IsDeleted = true };
            }
            else
            {
                return new DeleteCustomerResult { IsDeleted = false };
            }

        }

        private CustomerDeleted CustomerDeleted(Customer customer)
        {
            return new CustomerDeleted
            {
                Id = customer.Id,
            };
        }
    }
}
