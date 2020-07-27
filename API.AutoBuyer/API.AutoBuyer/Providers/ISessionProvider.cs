namespace AutoBuyer.API.Providers
{
    public interface ISessionProvider
    {
        string StartSession(string playerVersionId, string userId, int numToBuy);

        void EndSession(string sessionId, string playerVersionId, bool captcha, int numBought);
    }
}