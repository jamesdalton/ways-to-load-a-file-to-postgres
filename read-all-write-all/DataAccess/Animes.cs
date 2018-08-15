using System;
using System.Collections.Generic;
using ReadAllWriteAll.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace ReadAllWriteAll.DataAccess
{
    public class Animes
    {
        private IConfigurationRoot configuration;

        public Animes(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public void Save(List<Anime> animes)
        {
            using (var connection = new NpgsqlConnection(configuration["connectionString"]))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into anime (id, name, type_id, episodes, rating, members) values (@id, @name, @type_id, @episodes, @rating, @members)";
                    command.Parameters.Add(new NpgsqlParameter("id", NpgsqlTypes.NpgsqlDbType.Integer));
                    command.Parameters.Add(new NpgsqlParameter("name", NpgsqlTypes.NpgsqlDbType.Varchar));
                    command.Parameters.Add(new NpgsqlParameter("type_id", NpgsqlTypes.NpgsqlDbType.Integer));
                    command.Parameters.Add(new NpgsqlParameter("episodes", NpgsqlTypes.NpgsqlDbType.Integer));
                    command.Parameters.Add(new NpgsqlParameter("rating", NpgsqlTypes.NpgsqlDbType.Numeric));
                    command.Parameters.Add(new NpgsqlParameter("members", NpgsqlTypes.NpgsqlDbType.Integer));
                    foreach (var anime in animes)
                    {
                        command.Parameters[0].Value = anime.Id;
                        command.Parameters[1].Value = anime.Name;
                        command.Parameters[2].Value =  (object)anime.TypeId ?? DBNull.Value;
                        command.Parameters[3].Value =  (object)anime.Episodes ?? DBNull.Value;
                        command.Parameters[4].Value =  (object)anime.Rating ?? DBNull.Value;
                        command.Parameters[5].Value =  (object)anime.Members ?? DBNull.Value;
                        command.ExecuteNonQuery();                        
                    }
                }
            }
        }
    }
}