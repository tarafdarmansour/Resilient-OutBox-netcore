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
    public class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, UpdateCustomerResult>
    {
        private readonly ILogger<UpdateCustomerHandler> _logger;
        private readonly CustomerContext _context;
        private readonly IEventPublisher _eventPublisher;

        public UpdateCustomerHandler(ILogger<UpdateCustomerHandler> logger, CustomerContext context, IEventPublisher eventPublisher)
        {
            _logger = logger;
            _context = context;
            _eventPublisher = eventPublisher;
        }
        public async Task<UpdateCustomerResult> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {

            var entity = await _context.Customers.FindAsync(request.Id);
            if (entity != null)
            {
                entity.UpdateCustomer(new Customer(request.FirstName, request.LastName, request.BirthDate));

                _context.Customers.Update(entity);

                await _eventPublisher.PublishMessage(CustomerUpdated(entity));

                await _context.SaveChangesAsync(cancellationToken);

                return new UpdateCustomerResult { IsUpdated = true };
            }
            else
            {
                return new UpdateCustomerResult { IsUpdated = false };
            }

        }

        private CustomerUpdated CustomerUpdated(Customer customer)
        {
            return new CustomerUpdated
            {
                Id = customer.Id,
                BirthDate = customer.BirthDate,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        }
    }
}
