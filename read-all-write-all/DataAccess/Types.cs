using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace ReadAllWriteAll.DataAccess
{
    public class Types
    {
        private static Dictionary<string, int> types = new Dictionary<string, int>();
        private IConfigurationRoot configuration;

        public Types(IConfigurationRoot configuration)
        {
            this.configuration = configuration;
        }

        public int GetId(string type)
        {
            if (!types.ContainsKey(type))
            {
                using (var connection = new NpgsqlConnection(configuration["connectionString"]))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "insert into type (name) values (@name) returning id";
                        command.Parameters.Add(new NpgsqlParameter("name", type));
                        types[type] = (int)command.ExecuteScalar();
                    }
                }
            }
            return types[type];
        }
    }
}