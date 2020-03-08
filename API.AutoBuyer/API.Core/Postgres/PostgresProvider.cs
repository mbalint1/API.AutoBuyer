﻿using System;
using System.Collections.Generic;
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
                        cmd.CommandText = Queries.InsertTransactionLogs;
                        Queries.AddTransactionLogParams(cmd, log);
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
    }
}