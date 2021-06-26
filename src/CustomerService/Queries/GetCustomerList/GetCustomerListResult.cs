using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CustomerService.Queries
{
    public class GetCustomerListResult
    {
        public List<GetCustomerListResultItem> CustomerListResultItems { get; set; }
    }

    public class GetCustomerListResultItem
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string BirthDate { get; set; }
    }
}
