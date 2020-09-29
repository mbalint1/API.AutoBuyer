using System;
using AutoBuyer.API.Core.DTO;
using Npgsql;
using NpgsqlTypes;

namespace AutoBuyer.API.Core.Postgres
{
    public static class Queries
    {
        public static string InsertPlayers = @"INSERT INTO public.""Players"" (""Name"", ""Created_By"", ""Created_Date"", ""Modified_By"", ""Modified_Date"") VALUES (@name, @createdBy, @createdDate, @modifiedBy, @modifiedDate) RETURNING ""Player_Id"";";

        public static string InsertPlayerVersion = @"INSERT INTO public.""Player_Version"" (""Player_Id"", ""Fut_Id"", ""Player_Type"", ""Rating"", ""Position"", ""Created_By"", ""Created_Date"", ""Modified_By"", ""Modified_Date"") VALUES (@playerId, @futId, @playerType, @rating, @position, @createdBy, @createdDate, @modifiedBy, @modifiedDate) RETURNING ""Version_Id"";";

        public static string InsertUser = @"INSERT INTO public.""Users"" (""User_Name"", ""Email"", ""Password_Hash"", ""Lockout_Enabled"", ""Lockout_End_Date"", ""Access_Fail_Count"", ""Is_Temp_Password"", ""Created_By"", ""Created_Date"", ""Modified_By"", ""Modified_Date"") VALUES (@userName, @email, @passwordHash, @lockoutEnabled, @lockoutEndDate, @accessFailCount, @isTempPassword, @createdBy, @createdDate, @modifiedBy, @modifiedDate) RETURNING ""User_ID"";";

        public static string SelectUser = @"Select ""User_ID"", ""User_Name"", ""Email"", ""Password_Hash"", ""Lockout_Enabled"", ""Lockout_End_Date"", ""Access_Fail_Count"", ""Is_Temp_Password"", ""Created_By"", ""Created_Date"", ""Modified_By"", ""Modified_Date"" from public.""Users"" where lower(""User_Name"") = @userName;";
        
        public static string InsertTransactionHistory = @"INSERT INTO public.""Transaction_History"" (""Transaction_Type"", ""Player_Name"", ""Search_Price"", ""Sell_Price"", ""Transaction_Date"", ""User_Name"") VALUES (@transactionType, @playerName, @searchPrice, @sellPrice, @transactionDate, @userName) RETURNING ""Transaction_ID"";";

        public static string SelectPlayers = @"select p.""Name"", p.""Player_Id"", pv.""Version_Id"", pv.""Player_Type"", pv.""Rating"", pv.""Position"" from public.""Players"" p inner join public.""Player_Version"" pv on p.""Player_Id"" = pv.""Player_Id""";

        public static string UpdateSession = @"UPDATE public.""Buyer_Session"" set ""Active_Flag"" = 'N', ""End_Time"" = current_timestamp, ""Captcha_Flag"" = @captchaFlag, ""Purchased_Num"" = @numBought WHERE ""Player_Version_ID"" = @versionId AND ""Session_ID"" = @sessionId;";

        public static string SessionLockFunction = @"select public.lock_player(@p_version_id, @p_user_id, @p_num_buy);";

        public static void AddPlayerParameters(NpgsqlCommand cmd, Player player)
        {
            cmd.Parameters.AddWithValue("name", player.Name);
            cmd.Parameters.AddWithValue("createdBy", player.CreatedBy);
            cmd.Parameters.AddWithValue("createdDate", player.CreatedDate);
            cmd.Parameters.AddWithValue("modifiedBy", player.ModifiedBy);
            cmd.Parameters.AddWithValue("modifiedDate", player.ModifiedDate);
        }

        public static void AddVersionParameters(NpgsqlCommand cmd, Player player)
        {
            var version = player.Versions[0];

            cmd.Parameters.AddWithValue("playerId", Convert.ToInt32(player.Id));
            cmd.Parameters.AddWithValue("futId", Convert.ToInt32(version.ThirdPartyId));
            cmd.Parameters.AddWithValue("playerType", version.Type.ToString());
            cmd.Parameters.AddWithValue("rating", version.Rating);
            cmd.Parameters.AddWithValue("position", version.Position);
            cmd.Parameters.AddWithValue("createdBy", version.CreatedBy);
            cmd.Parameters.AddWithValue("createdDate", version.CreatedDate);
            cmd.Parameters.AddWithValue("modifiedBy", version.ModifiedBy);
            cmd.Parameters.AddWithValue("modifiedDate", version.ModifiedDate);
        }

        public static void AddTransactionHistoryParams(NpgsqlCommand cmd, TransactionLog log)
        {
            string postgresSucksAtEnums;

            switch (log.Type)
            {
                case TransactionType.SuccessfulPurchase:
                    postgresSucksAtEnums = "SuccessfulPurchase";
                    break;
                case TransactionType.FailedPurchase:
                    postgresSucksAtEnums = "FailedPurchase";
                    break;
                case TransactionType.SuccessfulSale:
                    postgresSucksAtEnums = "SuccessfulSale";
                    break;
                case TransactionType.FailedSale:
                    postgresSucksAtEnums = "FailedSale";
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            cmd.Parameters.AddWithValue("transactionType", postgresSucksAtEnums);
            cmd.Parameters.AddWithValue("playerName", log.PlayerName);
            cmd.Parameters.AddWithValue("searchPrice", log.SearchPrice);

            if (log.SellPrice == null)
            {
                cmd.Parameters.AddWithValue("sellPrice", DBNull.Value);
            }
            else
            {
                cmd.Parameters.AddWithValue("sellPrice", log.SellPrice);
            }

            cmd.Parameters.AddWithValue("transactionDate", log.TransactionDate);
            cmd.Parameters.AddWithValue("userName", log.UserName);
        }

        public static void AddUserInsertParams(NpgsqlCommand cmd, User user)
        {
            cmd.Parameters.AddWithValue("userName", user.UserName);
            cmd.Parameters.AddWithValue("email", user.Email);
            cmd.Parameters.AddWithValue("passwordHash", user.PasswordHash);
            cmd.Parameters.AddWithValue("lockoutEnabled", user.LockoutEnabled ? "Y" : "N");
            cmd.Parameters.AddWithValue("lockoutEndDate", user.LockoutEndDate); //TODO: Make nullable datetime and add conditional logic for value
            cmd.Parameters.AddWithValue("accessFailCount", user.AccessFailCount);
            cmd.Parameters.AddWithValue("isTempPassword", user.IsTempPassword ? "Y" : "N");
            cmd.Parameters.AddWithValue("createdBy", user.CreatedBy);
            cmd.Parameters.AddWithValue("createdDate", user.CreatedDate);
            cmd.Parameters.AddWithValue("modifiedBy", user.ModifiedBy);
            cmd.Parameters.AddWithValue("modifiedDate", user.ModifiedDate);
        }

        public static void AddSessionUpdateParams(NpgsqlCommand cmd, string sessionId, string playerVersionId, bool captcha, int numBought)
        {
            cmd.Parameters.AddWithValue("sessionId", Convert.ToInt32(sessionId));
            cmd.Parameters.AddWithValue("versionId", Convert.ToInt32(playerVersionId));
            cmd.Parameters.AddWithValue("captchaFlag", NpgsqlDbType.Char, captcha ? "Y" : "N");
            cmd.Parameters.AddWithValue("numBought", NpgsqlDbType.Integer, numBought);
        }

        public static void AddSessionCreateParams(NpgsqlCommand cmd, string versionId, string userId, int numToBuy)
        {
            cmd.Parameters.AddWithValue("p_version_id", NpgsqlDbType.Integer, Convert.ToInt32(versionId));
            cmd.Parameters.AddWithValue("p_user_id", NpgsqlDbType.Integer, Convert.ToInt32(userId));
            cmd.Parameters.AddWithValue("p_num_buy", NpgsqlDbType.Integer, numToBuy);
        }
    }
}