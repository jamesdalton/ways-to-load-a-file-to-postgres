using System;
using System.Collections.Generic;
using ReadAllCopyBinary.Models;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace ReadAllCopyBinary.DataAccess
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
                using (var writer = connection.BeginBinaryImport("COPY anime (id, name, type_id, episodes, rating, members) from STDIN (FORMAT BINARY)"))
                {
                    foreach (var anime in animes)
                    {
                        writer.StartRow();
                        writer.Write(anime.Id, NpgsqlTypes.NpgsqlDbType.Integer);
                        writer.Write(anime.Name, NpgsqlTypes.NpgsqlDbType.Varchar);
                        if (anime.TypeId.HasValue)
                            writer.Write(anime.TypeId.Value, NpgsqlTypes.NpgsqlDbType.Integer);
                        else
                            writer.WriteNull();
                        if (anime.Episodes.HasValue)
                            writer.Write(anime.Episodes.Value, NpgsqlTypes.NpgsqlDbType.Integer);
                        else
                            writer.WriteNull();
                        if (anime.Rating.HasValue)
                            writer.Write(anime.Rating.Value, NpgsqlTypes.NpgsqlDbType.Numeric);
                        else
                            writer.WriteNull();
                        if (anime.Members.HasValue)
                            writer.Write(anime.Members.Value, NpgsqlTypes.NpgsqlDbType.Integer);
                        else
                            writer.WriteNull();
                    }
                    writer.Complete();
                }
            }
        }
    }
}