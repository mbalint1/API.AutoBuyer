using AutoBuyer.API.Core.Utilities;
using NUnit.Framework;

namespace AutoBuyer.API.Tests
{
    [TestFixture]
    public class PasswordTests
    {
        [Test]
        public void VerifyHashTest()
        {
            const string password = "dontcry4meJanHrdina";

            var hash = PasswordUtility.HashPassword(password);

            var hashVerify = PasswordUtility.VerfiyHash(hash, password);

            Assert.IsTrue(hashVerify);

            var badPassword = PasswordUtility.VerfiyHash(hash, "Fake123");

            Assert.IsFalse(badPassword);
        }
    }
}