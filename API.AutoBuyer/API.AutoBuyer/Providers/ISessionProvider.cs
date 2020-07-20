namespace AutoBuyer.API.Providers
{
    public interface ISessionProvider
    {
        string StartSession(string playerVersionId, string userId);

        void EndSession(string sessionId, string playerVersionId);
    }
}