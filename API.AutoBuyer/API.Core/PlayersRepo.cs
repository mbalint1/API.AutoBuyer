using System.Collections.Generic;
using AutoBuyer.API.Core.DTO;
using AutoBuyer.API.Core.Interfaces;
using AutoBuyer.API.Core.Postgres;
using AutoBuyer.API.Core.Utilities;

namespace AutoBuyer.API.Core
{
    public class PlayersRepo : IRepo
    {
        public IDbProvider DbProvider { get; }

        public PlayersRepo()
        {
            var connString = ConnectionUtility.GetPostGresConnString();
            DbProvider = new PostgresProvider(connString);
        }

        public List<Player> GetPlayers()
        {
            return DbProvider.GetPlayers();
        }
    }
}