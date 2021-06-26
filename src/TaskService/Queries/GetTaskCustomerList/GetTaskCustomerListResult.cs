using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskService.Queries
{
    public class GetTaskCustomerListResult
    {
        public List<GetTaskCustomerListResultItem> TaskCustomerListResultItems { get; set; }
    }

    public class GetTaskCustomerListResultItem
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int TaskId { get; set; }
    }
}
