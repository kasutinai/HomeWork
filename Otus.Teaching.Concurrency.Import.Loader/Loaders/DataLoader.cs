using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Otus.Teaching.Concurrency.Import.Core.Loaders
{
    public class DataLoader
        : IDataLoader
    {
        private readonly List<Customer> _customers;
        public DataLoader(List<Customer> customers)
        {
            _customers = customers;
        }
        public void LoadData()
        {
            int numProcs    = Environment.ProcessorCount;
            int range       = _customers.Count / numProcs;
            var threads     = new List<Thread>(numProcs);

            for (int p = 0; p < numProcs; p++)
            {
                int start   = p * range;
                int end     = (p == numProcs - 1) ? _customers.Count : start + range;

                threads.Add(new Thread(() => {
                    var repository = new CustomerRepository();
                    for (int i = start; i < end; i++) repository.AddCustomer(_customers[i]);
                    repository.SaveChanges();
                }));
            }
            foreach (var thread in threads) thread.Start();
            foreach (var thread in threads) thread.Join();
        }
    }
}