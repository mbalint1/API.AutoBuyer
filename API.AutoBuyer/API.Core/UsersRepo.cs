using AutoBuyer.API.Core.DTO;
using AutoBuyer.API.Core.Interfaces;
using AutoBuyer.API.Core.Postgres;
using AutoBuyer.API.Core.Utilities;

namespace AutoBuyer.API.Core
{
    public class UsersRepo : IRepo
    {
        public IDbProvider DbProvider { get; }

        public UsersRepo()
        {
            var connString = ConnectionUtility.GetPostGresConnString();
            DbProvider = new PostgresProvider(connString);
        }

        public User InsertUser(User user)
        {
            DbProvider.InsertUser(user);

            return user;
        }

        public User GetUser(string userName)
        {
            //TODO: Finish
            return new User();
        }
    }
}