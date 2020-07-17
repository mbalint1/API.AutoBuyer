using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuyer.API.Core;
using AutoBuyer.API.Core.DTO;

namespace AutoBuyer.API.Providers
{
    public class PlayerProvider : IPlayerProvider
    {
        private PlayersRepo Repo { get; }

        public PlayerProvider()
        {
            Repo = new PlayersRepo();
        }

        public List<Player> GetPlayers()
        {
            return Repo.GetPlayers();
        }
    }
}
