﻿using System;
using System.Collections.Generic;
using AutoBuyer.API.Core.DTO;
using AutoBuyer.API.Core.Interfaces;
using AutoBuyer.API.Core.Postgres;
using AutoBuyer.API.Core.Utilities;

namespace AutoBuyer.API.Core
{
    public class TransactionRepo : IRepo
    {
        public IDbProvider DbProvider { get; }

        public TransactionRepo()
        {
            var connString = ConnectionUtility.GetPostGresConnString();
            DbProvider = new PostgresProvider(connString);
        }

        public void InsertTransactionLog(TransactionLog log)
        {
            DbProvider.InsertTransactionLog(log);
        }

        public List<TransactionLog> GetTransactions(string user, DateTime startDate, DateTime endDate)
        {
            return DbProvider.GetTransactions(user, startDate, endDate);
        }
    }
}