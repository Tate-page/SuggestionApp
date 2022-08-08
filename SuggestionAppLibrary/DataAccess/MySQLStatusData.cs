using Dapper;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace SuggestionAppLibrary.DataAccess
{
    public class MySQLStatusData : IStatusData
    {
        private readonly List<StatusModel> _statuses;
        private readonly IMemoryCache _cache;
        private const string CacheName = "StatusData";
        private readonly MySqlConnection _connection;
        public MySQLStatusData(IMemoryCache cache, IDbConnection conn)
        {
            _connection = conn.connection;
            _cache = cache;
            _statuses = getAllStatuses();
        }

        public List<StatusModel> getAllStatuses()
        {
            var output = _cache.Get<List<StatusModel>>(CacheName);

            if (output == null)
            {
                output = new List<StatusModel>();
                var temp = _connection.Query("CALL GetAllStatuses()").ToList();
                foreach (var stat in temp) {
                    StatusModel status = new()
                    {
                        Id = stat.StatusID.ToString(),
                        StatusName = stat.StatusName,
                        StatusDescription = stat.StatusDescription,
                    };
                    output.Add(status);
                }
                _cache.Set(CacheName, output, TimeSpan.FromDays(value: 1));
            }

            return output;
        }

        public async Task<List<StatusModel>> getAllStatusesAsync()
        {
            var output = _cache.Get<List<StatusModel>>(CacheName);

            if (output == null)
            {
                output = new List<StatusModel>();
                var temp = await _connection.QueryAsync("CALL GetAllStatuses()");
                foreach (var stat in temp)
                {
                    StatusModel status = new()
                    {
                        Id = stat.StatusID.ToString(),
                        StatusName = stat.StatusName,
                        StatusDescription = stat.StatusDescription,
                    };
                    output.Add(status);
                }
                _cache.Set(CacheName, output, TimeSpan.FromDays(value: 1));
            }

            return output;
        }

        public async Task CreateStatusAsync(StatusModel status)
        {
            await _connection.ExecuteAsync("CALL InsertStatus(@nStatusName, @nStatusDescription)", new {
                @nStatusName = status.StatusName,
                @nStatusDescription = status.StatusDescription 
            });
        }


    }
}
