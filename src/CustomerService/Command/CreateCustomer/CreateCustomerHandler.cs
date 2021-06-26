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
    public class CreateCustomerHandler : IRequestHandler<CreateCustomerCommand, CreateCustomerResult>
    {
        private readonly ILogger<CreateCustomerHandler> _logger;
        private readonly CustomerContext _context;
        private readonly IEventPublisher _eventPublisher;

        public CreateCustomerHandler(ILogger<CreateCustomerHandler> logger,CustomerContext context, IEventPublisher eventPublisher)
        {
            _logger = logger;
            _context = context;
            _eventPublisher = eventPublisher;
        }
        public async Task<CreateCustomerResult> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = new Customer(request.FirstName, request.LastName, request.BirthDate);

            await _context.Customers.AddAsync(customer,cancellationToken);

            await _eventPublisher.PublishMessage(CustomerCreated(customer));

            await _context.SaveChangesAsync(cancellationToken);

            return new CreateCustomerResult { CreateDateTime = customer.CreateDateTime, Id = customer.Id };
        }

        private CustomerCreated CustomerCreated(Customer customer)
        {
            return new CustomerCreated
            {
                Id = customer.Id,
                BirthDate = customer.BirthDate,
                FirstName = customer.FirstName,
                LastName = customer.LastName
            };
        }
    }
}
