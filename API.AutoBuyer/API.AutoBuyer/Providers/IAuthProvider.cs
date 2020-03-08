using AutoBuyer.API.Models;

namespace AutoBuyer.API.Providers
{
    public interface IAuthProvider
    {
        AuthResponse Authenticate(string user, string password);
    }
}