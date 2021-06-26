using AccountingService.AccountingDBContext;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccountingService.Queries
{
    public class GetAccountingCustomerListHandler : IRequestHandler<GetAccountingCustomerListQuery, GetAccountingCustomerListResult>
    {
        private readonly ILogger<GetAccountingCustomerListHandler> _logger;
        private readonly AccountingContext _context;

        public GetAccountingCustomerListHandler(ILogger<GetAccountingCustomerListHandler> logger, AccountingContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GetAccountingCustomerListResult> Handle(GetAccountingCustomerListQuery request, CancellationToken cancellationToken)
        {

            var customers = await _context.Customers.ToListAsync();

            GetAccountingCustomerListResult getCustomerListResult = new GetAccountingCustomerListResult
            {
                AccountingCustomerListResultItems = customers.Select(c => new GetAccountingCustomerListResultItem
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Balance = c.Balance
                })
                .ToList()
            };

            return getCustomerListResult;

        }

    }
}
