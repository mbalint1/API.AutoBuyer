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
    }
}