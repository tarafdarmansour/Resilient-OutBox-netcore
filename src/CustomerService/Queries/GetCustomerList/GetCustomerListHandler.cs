using CustomerService.CustomerDBContext;
using CustomerService.Events;
using CustomerService.Messaging;
using CustomerService.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerService.Queries
{
    public class GetCustomerListHandler : IRequestHandler<GetCustomerListQuery, GetCustomerListResult>
    {
        private readonly ILogger<GetCustomerListHandler> _logger;
        private readonly CustomerContext _context;

        public GetCustomerListHandler(ILogger<GetCustomerListHandler> logger, CustomerContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GetCustomerListResult> Handle(GetCustomerListQuery request, CancellationToken cancellationToken)
        {

            var customers = await _context.Customers.ToListAsync();

            GetCustomerListResult getCustomerListResult = new GetCustomerListResult
            {
                CustomerListResultItems = customers.Select(c => new GetCustomerListResultItem
                {
                    Id = c.Id,
                    BirthDate = c.BirthDate.ToString(),
                    FirstName = c.FirstName,
                    LastName = c.LastName
                })
                .ToList()
            };

            return getCustomerListResult;

        }

    }
}
