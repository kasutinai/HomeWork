using System;
using System.Linq;
using Otus.Teaching.Linq.ATM.Core.Services;
using Otus.Teaching.Linq.ATM.DataAccess;

namespace Otus.Teaching.Linq.ATM.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.OutputEncoding = System.Text.Encoding.UTF8;

            System.Console.WriteLine("Старт приложения-банкомата...");

            var atmManager = CreateATMManager();

            //TODO: Далее выводим результаты разработанных LINQ запросов
            //test data -->
            var loginToFind = "snow";
            var passwordToFind = "111";
            decimal accountAmount = 100000;
            // <--

            var curUser = atmManager.GetUserByLoginAndPassword(loginToFind, passwordToFind);

            if (curUser != null) 
            {
                System.Console.WriteLine($"User information\n{curUser.ToString()}");
                var curAccounts = atmManager.GetUserAccounts(curUser.Id);

                if (curAccounts.Count() > 0)
                {
                    System.Console.WriteLine("\nUser accounts");

                    foreach (var curAccount in curAccounts)
                        System.Console.WriteLine(curAccount.ToString());

                    System.Console.WriteLine("\nUser operations history");

                    var curHistories = atmManager.GetUserHistory(curUser.Id);
                    {
                        if (curHistories.Count() > 0)
                        {
                            foreach (var curHistory in curHistories)
                                System.Console.WriteLine(curHistory);
                        }
                        else { System.Console.WriteLine("None"); }
                    }
                }
            }

            var inputOps = atmManager.GetInputOperationsInfo();

            if (inputOps != null)
            {
                System.Console.WriteLine("\nInput operations:");

                foreach (var inputOp in inputOps)
                    System.Console.WriteLine(inputOp);
            }

            var filteredUsers = atmManager.GetUsersHavingAccountSumMoreThan(accountAmount);

            if (filteredUsers != null)
            {
                System.Console.WriteLine($"\nUsers having account bigger than {accountAmount}:");

                foreach (var filteredUser in filteredUsers)
                    System.Console.WriteLine(filteredUser);
            }

            System.Console.WriteLine("Завершение работы приложения-банкомата...");
        }

        static ATMManager CreateATMManager()
        {
            using var dataContext = new ATMDataContext();
            var users = dataContext.Users.ToList();
            var accounts = dataContext.Accounts.ToList();
            var history = dataContext.History.ToList();
                
            return new ATMManager(accounts, users, history);
        }
    }
}