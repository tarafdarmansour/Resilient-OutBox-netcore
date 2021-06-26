using System;

namespace CustomerService.Models
{

    public class Customer
    {
        public Customer(string firstname,string lastname,DateTime birthdate)
        {
            Id = Guid.NewGuid();
            FirstName = firstname;
            LastName = lastname;
            BirthDate = birthdate;
        }

        public Customer UpdateCustomer(Customer customer)
        {
            this.FirstName = customer.FirstName;
            this.LastName = customer.LastName;
            this.BirthDate = customer.BirthDate;
            this.UpdateDateTime = DateTime.Now;
            return this;
        }

        private Customer()
        {

        }

        public Guid Id { get; protected set; }
        public string FirstName { get; protected set; }
        public string LastName { get; protected set; }
        public DateTime BirthDate { get; protected set; }
        public DateTime CreateDateTime { get; protected set; }
        public DateTime? UpdateDateTime { get; protected set; }
    }

}