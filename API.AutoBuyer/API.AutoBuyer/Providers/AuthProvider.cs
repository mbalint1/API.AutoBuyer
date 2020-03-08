using System;
using AutoBuyer.API.Models;

namespace AutoBuyer.API.Providers
{
    public class AuthProvider : IAuthProvider
    {
        public AuthResponse Authenticate(string user, string password)
        {
            if (password == "2019ScrubADunkChampion!")
            {
                return new AuthResponse
                {
                    Authenticated = true,
                    Username = user,
                    AccessToken = "321sdf321asd3f21sad3f21as3df21",
                    TokenIssueDate = DateTime.Now,
                    TokenExpirationDate = DateTime.Now.AddDays(1)
                };
            }
            else
            {
                return new AuthResponse {Authenticated = false, Username = user};
            }
        }
    }
}