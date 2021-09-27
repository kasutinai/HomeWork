using Otus.Teaching.Concurrency.Import.DataAccess.Repositories;
using Otus.Teaching.Concurrency.Import.Handler.Entities;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Otus.Teaching.Concurrency.Import.Core.Loaders
{
    public class DataLoaderThreadPool
        : IDataLoader
    {
        private readonly List<Customer> _customers;
        public DataLoaderThreadPool(List<Customer> customers)
        {
            _customers = customers;
        }
        public void LoadData()
        {
            int numProcs    = Environment.ProcessorCount;
            int range       = _customers.Count / numProcs;
            int remaining   = numProcs;

            using (ManualResetEvent mre = new ManualResetEvent(false))
            {
                for (int p = 0; p < numProcs; p++)
                {
                    int start   = p * range;
                    int end     = (p == numProcs - 1) ? _customers.Count : start + range;

                    ThreadPool.QueueUserWorkItem(delegate {
                        var repository = new CustomerRepository();
                        for (int i = start; i < end; i++) repository.AddCustomer(_customers[i]);
                        repository.SaveChanges();
                        if (Interlocked.Decrement(ref remaining) == 0)
                            mre.Set();
                    });
                }

                mre.WaitOne();
            }
        }
    }
}