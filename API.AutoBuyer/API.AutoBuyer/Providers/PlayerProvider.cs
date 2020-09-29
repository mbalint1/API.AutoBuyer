using System.Collections.Generic;
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
