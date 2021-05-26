using System.Collections.Generic;
using System.Linq;
using Otus.Teaching.Linq.ATM.Core.Entities;

namespace Otus.Teaching.Linq.ATM.Core.Services
{
    public class ATMManager
    {
        public IEnumerable<Account> Accounts { get; private set; }

        public IEnumerable<User> Users { get; private set; }

        public IEnumerable<OperationsHistory> History { get; private set; }

        public ATMManager(IEnumerable<Account> accounts, IEnumerable<User> users, IEnumerable<OperationsHistory> history)
        {
            Accounts = accounts;
            Users = users;
            History = history;
        }

        //TODO: Добавить методы получения данных для банкомата
        public User GetUserByLoginAndPassword(string login, string password) => Users.Where(u => u.Login == login && u.Password == password).SingleOrDefault();
        public IEnumerable<Account> GetUserAccounts(int userId) => Accounts.Where(a => a.UserId == userId);
        public IEnumerable<OperationsHistory> GetAccountHistory(int accountId) => History.Where(h => h.AccountId == accountId);

        public IEnumerable<string> GetInputOperationsInfo()
        {
            var inputOps = from operation in History
                           where operation.OperationType == OperationType.InputCash
                           join account in Accounts on operation.AccountId equals account.Id
                           join user in Users on account.UserId equals user.Id
                           select $"{operation.OperationDate} {operation.CashSum} User {user.Id} {user.FirstName} {user.SurName }";
            return inputOps;
        }

        public IEnumerable<User> GetUsersHavingAccountSumMoreThan(decimal amount)
        {
            var users = from user in Users 
                        join account in Accounts.Where(a => a.CashAll > amount).Select(a => a.UserId).Distinct() on user.Id equals account
                        select user;

            return users;
        }
    }
}
