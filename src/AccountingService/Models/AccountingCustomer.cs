using CustomerService.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingService.Models
{
    public class AccountingCustomer
    {
        public AccountingCustomer()
        {

        }
        public AccountingCustomer(CustomerCreated customer)
        {
            var r = new Random();
            Id = customer.Id;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Balance = r.Next(10, 200);
            CreateDateTime = DateTime.Now;
        }

        public void UpdateAccountingCustomer(CustomerUpdated customer)
        {
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            UpdateDateTime = DateTime.Now;
        }

        public Guid Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public int Balance { get; protected set; }
        public DateTime CreateDateTime { get; protected set; }
        public DateTime? UpdateDateTime { get; protected set; }
    }

}
