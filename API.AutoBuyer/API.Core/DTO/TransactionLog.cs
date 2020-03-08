using System;

namespace AutoBuyer.API.Core.DTO
{
    public class TransactionLog
    {
        public string TransactionId { get; set; }

        public DateTime TransactionDate { get; set; }

        public TransactionType Type { get; set; }

        public string PlayerName { get; set; }

        public int SearchPrice { get; set; }
    }
}