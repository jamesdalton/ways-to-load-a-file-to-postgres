using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;

namespace ReadAllWriteOne.DataAccess
{
    public class AnimeGenres
    {
        private IConfigurationRoot configuration;

        public AnimeGenres(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public void Save(int animeId, List<int?> list)
        {
            using (var connection = new NpgsqlConnection(configuration["connectionString"]))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into anime_genre (anime_id, genre_id) values (:anime_id, :genre_id)";
                    command.Parameters.Add(new NpgsqlParameter("anime_id", NpgsqlTypes.NpgsqlDbType.Integer));
                    command.Parameters.Add(new NpgsqlParameter("genre_id", NpgsqlTypes.NpgsqlDbType.Integer));
                    foreach (var genre in list)
                    {
                        command.Parameters[0].Value = animeId;
                        command.Parameters[1].Value = genre;
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}