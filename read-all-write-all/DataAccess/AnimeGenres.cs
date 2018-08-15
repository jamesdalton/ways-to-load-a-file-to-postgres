using Npgsql;
using System.Collections.Generic;
using ReadAllWriteAll.Models;
using Microsoft.Extensions.Configuration;

namespace ReadAllWriteAll.DataAccess
{
    public class AnimeGenres
    {
        private IConfigurationRoot configuration;

        public AnimeGenres(IConfigurationRoot configuration)
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
                    command.CommandText = "insert into anime_genre (anime_id, genre_id) values (:anime_id, :genre_id)";
                    command.Parameters.Add(new NpgsqlParameter("anime_id", NpgsqlTypes.NpgsqlDbType.Integer));
                    command.Parameters.Add(new NpgsqlParameter("genre_id", NpgsqlTypes.NpgsqlDbType.Integer));
                    foreach (var anime in animes)
                    {
                        foreach (var genre in anime.GenreIds)
                        {
                            command.Parameters[0].Value = anime.Id;
                            command.Parameters[1].Value = genre;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }
    }
}