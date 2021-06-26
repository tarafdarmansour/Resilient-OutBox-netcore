using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TaskService.TaskDBContext;

namespace TaskService.Queries
{
    public class GetTaskCustomerListHandler : IRequestHandler<GetTaskCustomerListQuery, GetTaskCustomerListResult>
    {
        private readonly ILogger<GetTaskCustomerListHandler> _logger;
        private readonly TaskContext _context;

        public GetTaskCustomerListHandler(ILogger<GetTaskCustomerListHandler> logger, TaskContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<GetTaskCustomerListResult> Handle(GetTaskCustomerListQuery request, CancellationToken cancellationToken)
        {

            var customers = await _context.Customers.ToListAsync();

            GetTaskCustomerListResult getCustomerListResult = new GetTaskCustomerListResult
            {
                TaskCustomerListResultItems = customers.Select(c => new GetTaskCustomerListResultItem
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    TaskId = c.TaskId
                })
                .ToList()
            };

            return getCustomerListResult;

        }

    }
}
