using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Queries
{
    public class GetCustomerListQuery : IRequest<GetCustomerListResult>
    {
    }
}
