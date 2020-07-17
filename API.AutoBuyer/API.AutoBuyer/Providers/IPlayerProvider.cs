using System.Collections.Generic;
using AutoBuyer.API.Core.DTO;

namespace AutoBuyer.API.Providers
{
    public interface IPlayerProvider
    {
        List<Player> GetPlayers();
    }
}