using System;
using System.Collections.Generic;
using AutoBuyer.API.Core;
using AutoBuyer.API.Core.DTO;

namespace AutoBuyer.API.Providers
{
    public class TransactionProvider : ITransactionProvider
    {
        private TransactionRepo Repo { get; }

        public TransactionProvider()
        {
            Repo = new TransactionRepo();
        }

        public TransactionLog InsertTransactionLog(TransactionLog log)
        {
            Repo.InsertTransactionLog(log);

            return log;
        }

        public List<TransactionLog> GetTransactionLogs(string user, DateTime startDate, DateTime endDate)
        {
            return Repo.GetTransactions(user, startDate, endDate);
        }
    }
}