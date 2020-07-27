using System;
using AutoBuyer.API.Core;
using AutoBuyer.API.Core.DTO;
using AutoBuyer.API.Core.Utilities;
using AutoBuyer.API.Models;
using AutoBuyer.API.Providers;
using NUnit.Framework;

namespace AutoBuyer.API.Tests
{
    public class DataTests
    {
        [Test]
        public void InsertUser()
        {
            var userRepo = new UsersRepo();

            const string user = "";
            const string password = "";
            const string email = "";

            var hash = PasswordUtility.HashPassword(password);

            var userObject = new User
            {
                UserName = user,
                Email = email,
                PasswordHash = hash,
                IsTempPassword = false,
                LockoutEnabled = false,
                LockoutEndDate = DateTime.MaxValue,
                AccessFailCount = 0,
                CreatedBy = "",
                ModifiedBy = "",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var result = userRepo.InsertUser(userObject);

            Assert.IsFalse(string.IsNullOrEmpty(result.UserId));
        }

        [Test]
        public void InsertTransactionHistory()
        {
            var transactionRepo = new TransactionRepo();

            var log = new TransactionLog
            {
                Type = TransactionType.SuccessfulPurchase,
                PlayerName = "Lionel Messi",
                SearchPrice = 150000,
                //SellPrice = 175000,
                TransactionDate = DateTime.Now,
                UserName = "mbalint"
            };

            transactionRepo.InsertTransactionLog(log);

            Assert.IsTrue(log.TransactionId.Length > 0);
        }

        [Test]
        public void GetUser()
        {
            var result = new UsersRepo().GetUser("mbalint");
        }

        [Test]
        public void GetPlayers()
        {
            var players = new PlayersRepo().GetPlayers();

            Assert.IsTrue(players.Count > 0);
        }

        [Test]
        public void LockPlayer()
        {
            var result = new SessionProvider().StartSession("997", "1003", 15);
        }

        [Test]
        public void EndSession()
        {
            new SessionProvider().EndSession("1030", "997", true, 5);
        }
    }
}