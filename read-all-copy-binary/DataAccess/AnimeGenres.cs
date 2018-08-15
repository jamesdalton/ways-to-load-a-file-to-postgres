using Npgsql;
using System.Collections.Generic;
using ReadAllCopyBinary.Models;
using Microsoft.Extensions.Configuration;

namespace ReadAllCopyBinary.DataAccess
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
                using (var writer = connection.BeginBinaryImport("COPY anime_genre (anime_id, genre_id) FROM STDIN (FORMAT BINARY)"))
                {
                    foreach (var anime in animes)
                    {
                        foreach (var genre in anime.GenreIds)
                        {
                            writer.StartRow();
                            writer.Write(anime.Id, NpgsqlTypes.NpgsqlDbType.Integer);
                            writer.Write(genre.Value, NpgsqlTypes.NpgsqlDbType.Integer);
                        }
                    }
                    writer.Complete();
                }
            }
        }
    }
}