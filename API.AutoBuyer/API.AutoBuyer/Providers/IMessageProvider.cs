namespace AutoBuyer.API.Providers
{
    public interface IMessageProvider
    {
        void Send(string subject, string body, string emailTo);
    }
}