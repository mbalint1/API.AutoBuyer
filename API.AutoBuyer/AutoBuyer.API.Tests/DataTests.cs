using System;
using AutoBuyer.API.Core;
using AutoBuyer.API.Core.DTO;
using AutoBuyer.API.Core.Utilities;
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
                CreatedBy = "mbalint",
                ModifiedBy = "mbalint",
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now
            };

            var result = userRepo.InsertUser(userObject);

            Assert.IsFalse(string.IsNullOrEmpty(result.UserId));
        }
    }
}