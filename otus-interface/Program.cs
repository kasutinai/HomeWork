using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace otus_interfaces
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string s;
            Trace.Listeners.Add(new ConsoleTraceListener());

            var currencyConverter = new ExchangeRatesApiConverter(new HttpClient(), new MemoryCache(new MemoryCacheOptions()), "a5cf9da55cb835d0a633a7825b3aa8b5");

            var transactionRepository = new InFileTransactionRepository();
            //var transactionRepository = new InMemoryTransactionRepository();
            var transactionParser = new TransactionParser();

            var budgetApp = new BudjetApplication(transactionRepository, transactionParser, currencyConverter);

            /*budgetApp.AddTransaction("Трата -400 RUB Продукты Пятерочка");
            budgetApp.AddTransaction("Трата -2000 RUB Бензин IRBIS");
            budgetApp.AddTransaction("Трата -500 RUB Кафе Шоколадница");
            budgetApp.AddTransaction("Перевод 300 RUB Долг Илья");
            budgetApp.AddTransaction("Зачисление 1000 RUB Кэшбэк");*/

            
            while(true)
            {
                Console.WriteLine("Введите новую транзакцию или нажмите N для вывода итогов");
                //вводить надо в том же формате как выше закомменчено, иначе все рушится, эту обработку не делал
                s = Console.ReadLine();

                if (s != "N")
                    budgetApp.AddTransaction(s);
                else break;
            }

            budgetApp.OutputTransactions();

            budgetApp.OutputBalanceInCurrency("USD");

            Console.Read();
         }
    }
}
