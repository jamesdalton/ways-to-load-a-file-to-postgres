using Async.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Async.DataAccess
{
    public class AnimeStaging
    {
        private IConfigurationRoot configuration;
        private Genres genres;
        private Types types;

        public AnimeStaging(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
            genres = new Genres(configuration);
            types = new Types(configuration);
        }

        private async Task<Anime> ReadAsync(DbDataReader reader)
        {
            var hasNextRow = await reader.ReadAsync();
            if (!hasNextRow)
            {
                return null;
            }
            var id = reader.GetInt32(0);
            var episodes = reader.GetString(4);
            return new Anime
            {
                Id = id,
                Name = reader.GetString(1),
                GenreIds = reader.IsDBNull(2) ? new List<int?>() : reader.GetString(2).Split(',').Select(x => genres.GetId(x.Trim())).Distinct().ToList(),
                TypeId = reader.IsDBNull(3) ? (int?)null : types.GetId(reader.GetString(3)),
                Episodes = episodes == "Unknown" ? (int?)null : Int32.Parse(episodes),
                Rating = reader.IsDBNull(5) ? (double?)null : reader.GetDouble(5),
                Members = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6)
            };
        }

        public IEnumerable<Task<Anime>> Read()
        {
            using (var connection = new NpgsqlConnection(configuration["connectionString"]))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select anime_id, name, genres, type, episodes, rating, members from anime_staging";
                    using (var reader = command.ExecuteReader())
                    {
                        var row = ReadAsync(reader);
                        while (reader.IsOnRow)
                        {
                            yield return row;
                            row = ReadAsync(reader);
                        }
                    }
                }
            }
        }
    }
}