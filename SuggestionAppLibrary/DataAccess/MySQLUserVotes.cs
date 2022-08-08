using Dapper;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace SuggestionAppLibrary.DataAccess
{
    public class MySQLUserVotes : IUserVotes
    {
        private readonly MySqlConnection _connection;
        public MySQLUserVotes(IDbConnection conn)
        {
            _connection = conn.connection;
        }
        public async Task<List<UserVotesModel>> GetUpvotesByUserIdAsync(string nID)
        {
            List<UserVotesModel> votes = new List<UserVotesModel>();
            var output = await _connection.QueryAsync("CALL GetUserByID(@Id)",
                new
                {
                    @Id = nID
                }
            );
            foreach (var vote in output)
            {
                UserVotesModel userVotesModel = new UserVotesModel()
                {
                    UserID = vote.UserID,
                    SuggestionID = vote.UpvoteSuggestionID
                };
                votes.Add(userVotesModel);
            }
            return votes;
        }

    }
}
