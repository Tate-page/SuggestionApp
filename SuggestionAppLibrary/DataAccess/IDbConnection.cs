using MySql.Data.MySqlClient;

namespace SuggestionAppLibrary.DataAccess
{
    public interface IDbConnection
    {
        MySqlConnection connection { get; }
        string DbName { get; }
    }
}