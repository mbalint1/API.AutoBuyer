using AutoBuyer.API.Core.Postgres;

namespace AutoBuyer.API.Core.Interfaces
{
    public interface IRepo
    {
        IDbProvider DbProvider { get; }
    }
}