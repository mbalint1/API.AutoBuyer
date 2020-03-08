﻿using System;
using AutoBuyer.API.Core.DTO;
using Npgsql;

namespace AutoBuyer.API.Core.Postgres
{
    public static class Queries
    {
        public static string InsertPlayers = @"INSERT INTO public.""Players"" (""Name"", ""Created_By"", ""Created_Date"", ""Modified_By"", ""Modified_Date"") VALUES (@name, @createdBy, @createdDate, @modifiedBy, @modifiedDate) RETURNING ""Player_Id"";";

        public static string InsertPlayerVersion = @"INSERT INTO public.""Player_Version"" (""Player_Id"", ""Fut_Id"", ""Player_Type"", ""Rating"", ""Position"", ""Created_By"", ""Created_Date"", ""Modified_By"", ""Modified_Date"") VALUES (@playerId, @futId, @playerType, @rating, @position, @createdBy, @createdDate, @modifiedBy, @modifiedDate) RETURNING ""Version_Id"";";

        public static string InsertTransactionLogs = @"INSERT INTO public.""Transaction_Log"" (""Transaction_Type"", ""Player_Name"", ""Search_Price"", ""Transaction_Date"", ""Created_By"") VALUES (@transactionType, @playerName, @searchPrice, @transactionDate, @createdBy) RETURNING ""Transaction_ID"";";

        public static string InsertUser = @"INSERT INTO public.""Users"" (""User_Name"", ""Email"", ""Password_Hash"", ""Lockout_Enabled"", ""Lockout_End_Date"", ""Access_Fail_Count"", ""Is_Temp_Password"", ""Created_By"", ""Created_Date"", ""Modified_By"", ""Modified_Date"") VALUES (@userName, @email, @passwordHash, @lockoutEnabled, @lockoutEndDate, @accessFailCount, @isTempPassword, @createdBy, @createdDate, @modifiedBy, @modifiedDate) RETURNING ""User_ID"";";

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

        public static void AddTransactionLogParams(NpgsqlCommand cmd, TransactionLog log)
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
            cmd.Parameters.AddWithValue("transactionDate", log.TransactionDate);
            cmd.Parameters.AddWithValue("createdBy", "AutoBuyer.LogParser");
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
    }
}