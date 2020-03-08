using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoBuyer.API.Core;
using AutoBuyer.API.Core.Utilities;
using AutoBuyer.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace AutoBuyer.API.Providers
{
    public class AuthProvider : IAuthProvider
    {
        public AuthResponse Authenticate(string user, string password)
        {
            var userData = new UsersRepo().GetUser(user.Trim());

            var goodPassword = PasswordUtility.VerfiyHash(userData.PasswordHash, password.Trim());

            if (goodPassword)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenIssueDate = DateTime.Now;
                var tokenExpiration = DateTime.Now.AddHours(12);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new List<Claim> {new Claim("User", user.Trim().ToLower())}),
                    Expires = tokenExpiration,
                    //TODO: Get this out of a config or DB
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes("JakeGuentzelEatsFriedGreenTomatoes")), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                return new AuthResponse
                {
                    Authenticated = true,
                    Username = user,
                    AccessToken = tokenHandler.WriteToken(token),
                    TokenIssueDate = tokenIssueDate,
                    TokenExpirationDate = tokenExpiration
                };
            }
            else
            {
                return new AuthResponse {Authenticated = false, Username = user};
            }
        }
    }
}