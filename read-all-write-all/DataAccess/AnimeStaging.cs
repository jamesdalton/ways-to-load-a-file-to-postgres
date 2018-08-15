using ReadAllWriteAll.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace ReadAllWriteAll.DataAccess
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

        public List<Anime> Read()
        {
            using (var connection = new NpgsqlConnection(configuration["connectionString"]))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "select anime_id, name, genres, type, episodes, rating, members from anime_staging";
                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<Anime>();
                        while (reader.Read())
                        {
                            var id = reader.GetInt32(0);
                            var episodes = reader.GetString(4);
                            list.Add(new Anime
                            {
                                Id = id,
                                Name = reader.GetString(1),
                                GenreIds = reader.IsDBNull(2) ? new List<int?>() : reader.GetString(2).Split(',').Select(x => genres.GetId(x.Trim())).Distinct().ToList(),
                                TypeId = reader.IsDBNull(3) ? (int?)null : types.GetId(reader.GetString(3)),
                                Episodes = episodes == "Unknown" ? (int?)null : Int32.Parse(episodes),
                                Rating = reader.IsDBNull(5) ? (double?)null : reader.GetDouble(5),
                                Members = reader.IsDBNull(6) ? (int?)null : reader.GetInt32(6)
                            });
                        }
                        return list;
                    }
                }
            }
        }
    }
}