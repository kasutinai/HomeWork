using System;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using Otus.Teaching.Concurrency.Import.Handler.Repositories;
using Otus.Teaching.Concurrency.Import.DataAccess.Context;
using System.Threading.Tasks;

namespace Otus.Teaching.Concurrency.Import.DataAccess.Repositories
{
    public class CustomerRepository
        : ICustomerRepository
    {
        private readonly CustomerContext _db = new CustomerContext();
        public void AddCustomer(Customer customer)
        {
            _db.Customers.Add(customer);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
         }

        public void Reset()
        {
            _db.Customers.RemoveRange(_db.Customers);
            _db.SaveChanges();
        }
    }
}