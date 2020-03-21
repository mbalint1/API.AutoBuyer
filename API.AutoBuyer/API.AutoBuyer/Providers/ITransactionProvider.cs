using System;
using System.Collections.Generic;
using AutoBuyer.API.Core.DTO;

namespace AutoBuyer.API.Providers
{
    public interface ITransactionProvider
    {
        TransactionLog InsertTransactionLog(TransactionLog log);
        List<TransactionLog> GetTransactionLogs(string user, DateTime startDate, DateTime endDate);
    }
}