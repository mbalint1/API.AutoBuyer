using System.Collections.Generic;
using AutoBuyer.API.Core.DTO;

namespace AutoBuyer.API.Core.Interfaces
{
    public interface IDbProvider
    {
        void InsertPlayers(List<Player> players);

        void InsertTransactionLog(TransactionLog log);

        void InsertUser(User user);

        User GetUser(string userName);

        List<Player> GetPlayers();

        string TryLockPlayer(string playerVersionId, string userId, int numToBuy);

        void EndSession(string sessionId, string playerVersionId, bool captcha, int numBought);
    }
}