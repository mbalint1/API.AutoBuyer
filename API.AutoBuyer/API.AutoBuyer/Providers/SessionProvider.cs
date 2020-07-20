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

        public string StartSession(string playerVersionId, string userId)
        {
            return Repo.StartSession(playerVersionId, userId);
        }

        public void EndSession(string sessionId, string playerVersionId)
        {
            Repo.EndSession(sessionId, playerVersionId);
        }
    }
}