using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Command
{
    public class CreateCustomerResult
    {
        public Guid Id { get; set; }
        public DateTime CreateDateTime { get; set; }
    }
}
