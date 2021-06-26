using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingService.Queries
{
    public class GetAccountingCustomerListResult
    {
        public List<GetAccountingCustomerListResultItem> AccountingCustomerListResultItems { get; set; }
    }

    public class GetAccountingCustomerListResultItem
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Balance { get; set; }
    }
}
