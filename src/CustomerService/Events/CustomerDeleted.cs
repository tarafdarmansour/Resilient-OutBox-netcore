using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Events
{
    public class CustomerDeleted : INotification
    {
        public Guid Id { get; set; }
    }
}
