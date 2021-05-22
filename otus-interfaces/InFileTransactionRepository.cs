using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace otus_interfaces
{
    public class InFileTransactionRepository : ITransactionRepository
    {
        private const string filePath = @"C:\FU.txt";
        private readonly List<ITransaction> _transactions = new List<ITransaction>();

        public void AddTransaction(ITransaction transaction)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, true))
                {
                    sw.WriteLine(transaction.ToStringDB());
                    sw.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public ITransaction[] GetTransactions()
        {
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    var transactionParser = new TransactionParser();
                    while ((line = sr.ReadLine()) != null)
                    {
                        var transaction = transactionParser.Parse(line);
                        _transactions.Add(transaction);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
            return _transactions.ToArray();
        }
    }
}