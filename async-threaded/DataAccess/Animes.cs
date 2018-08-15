using System;
using System.Threading.Tasks;
using Async.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Async.DataAccess
{
    public class Animes
    {
        private IConfigurationRoot configuration;

        public Animes(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public async Task Save(Anime anime)
        {
            using (var connection = new NpgsqlConnection(configuration["connectionString"]))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into anime (id, name, type_id, episodes, rating, members) values (@id, @name, @type_id, @episodes, @rating, @members)";
                    command.Parameters.Add(new NpgsqlParameter("id", anime.Id));
                    command.Parameters.Add(new NpgsqlParameter("name", anime.Name));
                    command.Parameters.Add(new NpgsqlParameter("type_id", (object)anime.TypeId ?? DBNull.Value));
                    command.Parameters.Add(new NpgsqlParameter("episodes", (object)anime.Episodes ?? DBNull.Value));
                    command.Parameters.Add(new NpgsqlParameter("rating", (object)anime.Rating ?? DBNull.Value));
                    command.Parameters.Add(new NpgsqlParameter("members", (object)anime.Members ?? DBNull.Value));
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}