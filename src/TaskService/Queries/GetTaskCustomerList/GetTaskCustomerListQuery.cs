using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.Queries
{
    public class GetTaskCustomerListQuery : IRequest<GetTaskCustomerListResult>
    {
    }
}
