using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using MySql.Data;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Dapper;
using System.Threading.Tasks;

namespace SuggestionAppLibrary.DataAccess
{
    public class DbConnection : IDbConnection
    {
        private readonly IConfiguration _config;

        private string _connectionId = "Default";


        public string DbName { get; private set; }

        public MySqlConnection connection { get; private set; }


        public DbConnection(IConfiguration config)
        {
            _config = config;
            DbName = _config[key: "DatabaseName"];
            connection = new MySql.Data.MySqlClient.MySqlConnection(_config.GetConnectionString("Default"));
        }
    }
}
