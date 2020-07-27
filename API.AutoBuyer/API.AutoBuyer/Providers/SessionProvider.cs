using AutoBuyer.API.Core;

namespace AutoBuyer.API.Providers
{
    public class SessionProvider : ISessionProvider
    {
        private UsersRepo Repo { get; }

        public SessionProvider()
        {
            Repo = new UsersRepo();
        }

        public string StartSession(string playerVersionId, string userId, int numToBuy)
        {
            return Repo.StartSession(playerVersionId, userId, numToBuy);
        }

        public void EndSession(string sessionId, string playerVersionId, bool captcha, int numBought)
        {
            Repo.EndSession(sessionId, playerVersionId, captcha, numBought);
        }
    }
}