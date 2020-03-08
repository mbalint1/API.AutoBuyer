using System;

namespace AutoBuyer.API.Core.DTO
{
    public class User
    {
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string PasswordHash { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTime LockoutEndDate { get; set; }

        public int AccessFailCount { get; set; }

        public bool IsTempPassword { get; set; }

        public string CreatedBy { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}