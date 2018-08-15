using System.Collections.Generic;
using System.Threading;
using Npgsql;
using Microsoft.Extensions.Configuration;

namespace Async.DataAccess
{
    public class Types
    {

        private IConfigurationRoot configuration;
        private Dictionary<string, int> types = new Dictionary<string, int>();

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