using System;

namespace otus_interfaces
{
    public class Comission : ITransaction // декоратор
    {
        public ITransaction OriginalTransaction { get; }
        public ICurrencyAmount Amount { get; }
        public DateTimeOffset Date { get; }

        public override string ToString() => $"Комиссия в размере {Amount} за транзакцию: {OriginalTransaction}";
        public string ToStringDB() => $"Комиссия {Amount.Amount} {Amount.CurrencyCode} {Date} {OriginalTransaction}";
    }
}
