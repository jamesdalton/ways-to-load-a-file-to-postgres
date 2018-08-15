using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections.Generic;

namespace ReadAllCopyBinary.DataAccess
{
    public class Genres
    {
        private Dictionary<string, int> genres = new Dictionary<string, int>();
        private IConfigurationRoot configuration;

        public Genres(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public int? GetId(string genre)
        {
            if (!genres.ContainsKey(genre))
            {
                using (var connection = new NpgsqlConnection(configuration["connectionString"]))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "insert into genre (name) values (@name) returning id";
                        command.Parameters.Add(new NpgsqlParameter("name", genre));
                        genres[genre] = (int)command.ExecuteScalar();
                    }
                }
            }
            return genres[genre];
        }
    }
}