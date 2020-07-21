using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using AutoBuyer.API.Core.DTO;
using AutoBuyer.API.Core.Interfaces;
using Npgsql;

namespace AutoBuyer.API.Core.Postgres
{
    public class PostgresProvider : IDbProvider
    {
        private readonly string _connString;

        public PostgresProvider(string connString)
        {
            _connString = connString;
        }

        public void InsertPlayers(List<Player> players)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    for (int i = 0; i < players.Count; i++)
                    {
                        var player = players[i];

                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = Queries.InsertPlayers;
                            Queries.AddPlayerParameters(cmd, player);
                            if (i == 0)
                            {
                                cmd.Prepare();
                            }

                            var playerId = cmd.ExecuteScalar()?.ToString();

                            player.Id = playerId;
                        }

                        using (var cmd = new NpgsqlCommand())
                        {
                            cmd.Connection = conn;
                            cmd.CommandText = Queries.InsertPlayerVersion;
                            Queries.AddVersionParameters(cmd, player);
                            if (i == 0)
                            {
                                cmd.Prepare();
                            }

                            player.Versions.First().VersionId = cmd.ExecuteScalar()?.ToString();
                        }


                    }
                }
            }
            catch (Exception ex)
            {
                //TODO Log
                throw;
            }
        }

        public void InsertTransactionLog(TransactionLog log)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = Queries.InsertTransactionHistory;
                        Queries.AddTransactionHistoryParams(cmd, log);
                        cmd.Prepare();
                        var transactionId = cmd.ExecuteScalar()?.ToString();
                        log.TransactionId = transactionId;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO Log
                throw;
            }
        }

        public void InsertUser(User user)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = Queries.InsertUser;
                        Queries.AddUserInsertParams(cmd, user);
                        cmd.Prepare();
                        var userId = cmd.ExecuteScalar()?.ToString();
                        user.UserId = userId;
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO Log
                throw;
            }
        }

        public User GetUser(string userName)
        {
            var user = new User();

            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        var query = Queries.SelectUser.Replace(@"@userName", $"'{userName.Trim().ToLower()}'");
                        cmd.CommandText = query;

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();

                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                user.PasswordHash = dt.Rows[0]["Password_Hash"].ToString();
                                user.UserId = dt.Rows[0]["User_ID"].ToString();

                                //TODO: Add the other columns
                            }
                        }

                    }
                }
            }
            catch (Exception e)
            {
                //TODO Log
                throw;
            }

            return user;
        }

        public List<Player> GetPlayers()
        {
            var players = new List<Player>();

            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = Queries.SelectPlayers;

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();

                            adapter.Fill(dt);

                            if (dt.Rows.Count > 0)
                            {
                                foreach (DataRow row in dt.Rows)
                                {
                                    players.Add(new Player
                                    {
                                        Name = row["Name"].ToString(),
                                        Id = row["Player_Id"].ToString(),
                                        //TODO: This will have to change once we allow for multiple versions of a player
                                        Versions = new List<PlayerVersion> { new PlayerVersion { VersionId = row["Version_Id"].ToString(), Rating = Convert.ToInt32(row["Rating"].ToString()), Position = row["Position"].ToString()} }
                                    });
                                }
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: Log
                throw;
            }

            return players;
        }

        public string TryLockPlayer(string playerVersionId, string userId)
        {
            try
            {
                string id;

                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = Queries.SessionLockFunction;
                        cmd.CommandType = CommandType.StoredProcedure;

                        var response = cmd.ExecuteScalar();
                        id = response.ToString();
                    }
                }

                return id;
            }
            catch (Exception ex)
            {
                //TODO Log
                throw;
            }
        }

        public void EndSession(string sessionId, string playerVersionId)
        {
            try
            {
                using (var conn = new NpgsqlConnection(_connString))
                {
                    conn.Open();

                    using (var cmd = new NpgsqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = Queries.UpdateSession;
                        Queries.AddSessionUpdateParams(cmd, sessionId, playerVersionId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO Log
                throw;
            }
        }
    }
}