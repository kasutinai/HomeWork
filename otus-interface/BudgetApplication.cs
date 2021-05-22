using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace otus_interfaces
{
    public class BudjetApplication : IBudgetApplication
    {
        private readonly ICurrencyConverter _currencyConverter;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionParser _transactionParser;        

        public BudjetApplication(ITransactionRepository transactionRepository, ITransactionParser transactionParser, ICurrencyConverter currencyConverter)
        {
            _currencyConverter = currencyConverter;
            _transactionRepository = transactionRepository;
            _transactionParser = transactionParser;            
        }

        public void AddTransaction(string input)
        {
            var transaction = _transactionParser.Parse(input);
            _transactionRepository.AddTransaction(transaction);
        }

        public void OutputTransactions()
        {
            Console.WriteLine("История транзакций:");
            foreach (var transaction in _transactionRepository.GetTransactions())
            {
                Console.WriteLine(transaction);
            }
        }

        public void OutputBalanceInCurrency(string currencyCode)
        {
            var totalCurrencyAmount = new CurrencyAmount(currencyCode, 0);
            var amounts = _transactionRepository.GetTransactions()
                .Select(t => t.Amount)
                .Select(a => a.CurrencyCode != currencyCode ? _currencyConverter.ConvertCurrency(a, currencyCode) : a)
                .ToArray();

            var totalBalanceAmount = amounts.Aggregate(totalCurrencyAmount, (t, a) => t += a);

            Console.WriteLine($"Balance: {totalBalanceAmount}");
        }
    }
}
