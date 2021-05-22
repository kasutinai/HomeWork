using System;

namespace otus_interfaces
{
    public class Transfer : ITransaction
    {
        public Transfer(ICurrencyAmount amount, DateTimeOffset date, string message, string destination)
        {
            Amount = amount;
            Date = date;
            Message = message;
            Destination = destination;
        }
        public ICurrencyAmount Amount { get; }
        public DateTimeOffset Date { get; }

        public string Destination { get; }
        public string Message { get; }

        public override string ToString() => $"Перевод {Amount} на имя {Destination} с сообщением {Message}";
        public string ToStringDB() => $"Перевод {Amount} {Message} {Destination}";
    }
}