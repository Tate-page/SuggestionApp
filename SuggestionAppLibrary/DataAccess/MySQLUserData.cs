using Dapper;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;
using Microsoft.Extensions.Caching.Memory;

namespace SuggestionAppLibrary.DataAccess
{
    public class MySQLUserData : IUserData
    {
        private const string CacheName = "loggedInUser";
        private readonly MySqlConnection _connection;
        private readonly IMemoryCache _cache;
        public MySQLUserData(IDbConnection conn, IMemoryCache cache)
        {
            _connection = conn.connection;
            _cache = cache;
        }

        public async Task<List<UserModel>> GetUsersAsync()
        {
            //var output = connection.Query<Person>($"SELECT * FROM Person WHERE Lname ='{ lastName }'").ToList();
            List<UserModel> output = new();
            var temp = await _connection.QueryAsync("CALL GetUsers()");
            foreach (var item in temp) {
                UserModel user = new()
                {
                    Id = item.UserID.ToString(),
                    FirstName = item.Fname,
                    LastName = item.LName,
                    DisplayName = item.DisplayName,
                    EmailAddress = item.Email,
                    AuthoredSuggestion = await this.getAuthoredSuggestionsByIDAsync(item.UserID.ToString()),
                    VotedOnSuggestions = await this.getUpvotesByUserIDAsync(item.UserID.ToString())
                };
                output.Add(user);
            }
            return output;
        }

        public async Task<List<BasicUserModel>> GetBasicUsersAsync()
        {
            //var output = connection.Query<Person>($"SELECT * FROM Person WHERE Lname ='{ lastName }'").ToList();
            List<BasicUserModel> output = new();
            var temp = await _connection.QueryAsync("CALL GetUsers()");
            foreach (var item in temp)
            {
                BasicUserModel user = new()
                {
                    Id = item.UserID.ToString(),
                    DisplayName = item.DisplayName

                };
                output.Add(user);
            }
            return output;
        }

        public List<BasicUserModel> GetBasicUsers()
        {
            //var output = connection.Query<Person>($"SELECT * FROM Person WHERE Lname ='{ lastName }'").ToList();
            List<BasicUserModel> output = new();
            var temp = _connection.Query("CALL GetUsers()");
            foreach (var item in temp)
            {
                BasicUserModel user = new()
                {
                    Id =  item.UserID.ToString(),
                    DisplayName = item.DisplayName

                };
                output.Add(user);
            }
            return output;
        }

        public async Task<UserModel> GetUserFromIdAsync(string nId)
        {
            //var output = connection.Query<Person>($"SELECT * FROM Person WHERE Lname ='{ lastName }'").ToList();
            var tamp = await _connection.QueryFirstOrDefaultAsync("CALL GetUserByID(@Id)",
                new {
                    @Id = nId    
                }
            );

            UserModel user = new()
            {
                Id = tamp.UserID.ToString(),
                FirstName = tamp.Fname,
                LastName = tamp.LName,
                DisplayName = tamp.DisplayName,
                EmailAddress = tamp.Email,
                AuthoredSuggestion = await this.getAuthoredSuggestionsByIDAsync(tamp.UserID.ToString()),
                VotedOnSuggestions = await this.getUpvotesByUserIDAsync(tamp.UserID.ToString())
            };

            return user;
        }

        public async Task<UserModel> GetLastCreatedUserAsync() {
            var tamp = await _connection.QueryFirstOrDefaultAsync("CALL getLatestCreatedUser()");
            UserModel user = new() {
                Id = tamp.UserID.ToString(),
                FirstName = tamp.Fname, 
                LastName = tamp.LName,
                DisplayName = tamp.DisplayName,
                EmailAddress = tamp.Email,
                AuthoredSuggestion = await this.getAuthoredSuggestionsByIDAsync(tamp.UserID.ToString()),
                VotedOnSuggestions = await this.getUpvotesByUserIDAsync(tamp.UserID.ToString())
            };
            return user;
        }

        public async Task CreateUserAsync(UserModel user, string password, string favoriteColor)
        {
            await _connection.ExecuteAsync("CALL InsertUser(@nFname, @nLname, @DisplayName, @nEmail, @nPassword)",
                new
                {
                    @nFname = user.FirstName,
                    @nLname = user.LastName,
                    @DisplayName = user.DisplayName,
                    @nEmail = user.EmailAddress,
                    @nPassword = password,
                    @nFavoriteColor = favoriteColor
                }
            );
        }

        public void CreateUser(UserModel user, string password, string favoriteColor)
        {
            _connection.Execute("CALL InsertUser(@nFname, @nLname, @DisplayName, @nEmail, @nPassword, @nFavoriteColor)",
                new
                {
                    @nFname = user.FirstName,
                    @nLname = user.LastName,
                    @DisplayName = user.DisplayName,
                    @nEmail = user.EmailAddress,
                    @nPassword = password,
                    @nFavoriteColor = favoriteColor
                }
            );
        }

        public async Task updateUserAsync(UserModel user)
        {
            await _connection.ExecuteAsync("CALL UpdateUser(@nFname, @nLname, @DisplayName, @nEmail)",
                new
                {
                    @nFname = user.FirstName,
                    @nLname = user.LastName,
                    @nDisplayName = user.DisplayName,
                    @nEmail = user.EmailAddress
                }
            );
        }

        public async Task<List<BasicSuggestionModel>> getAuthoredSuggestionsByIDAsync(string id) {
            List<BasicSuggestionModel> output = new List<BasicSuggestionModel>();

            var temp = await _connection.QueryAsync("CALL getBasicSuggestionsByAuthoredID(@nId)",
                new {
                    @nId = id
                }
            );

            foreach (var item in temp) {
                BasicSuggestionModel suggestion = new()
                {
                    Id = item.SuggestionID.ToString(),
                    Suggestion = item.Suggestion
                };
                output.Add(suggestion);
            }
            return output;
        }

        public List<BasicSuggestionModel> getAuthoredSuggestionsByID(string id)
        {
            List<BasicSuggestionModel> output = new List<BasicSuggestionModel>();

            var temp = _connection.Query("CALL getBasicSuggestionsByAuthoredID(@nId)",
                new
                {
                    @nId = id
                }
            );

            foreach (var item in temp)
            {
                BasicSuggestionModel suggestion = new()
                {
                    Id = item.SuggestionID.ToString(),
                    Suggestion = item.Suggestion
                };
                output.Add(suggestion);
            }
            return output;
        }

        public async Task<List<UserVotesModel>> getUpvotesByUserIDAsync(string id) {
            List<UserVotesModel> output = new List<UserVotesModel>();

            var temp = await _connection.QueryAsync("CALL getUpvotesByUserID(@nId)",
                new
                {
                    @nId = id
                }
            );

            foreach (var item in temp)
            {
                UserVotesModel suggestion = new()
                {
                    UserID = item.UserID.ToString(),
                    SuggestionID = item.SuggestionID.ToString()
                };
                output.Add(suggestion);
            }
            return output;
        }

        public List<UserVotesModel> getUpvotesByUserID(string id)
        {
            List<UserVotesModel> output = new List<UserVotesModel>();

            var temp = _connection.Query("CALL getUpvotesByUserID(@nId)",
                new
                {
                    @nId = Int32.Parse(id)
                }
            );

            foreach (var item in temp)
            {
                UserVotesModel suggestion = new()
                {
                    UserID = item.UserID.ToString(),
                    SuggestionID = item.UpvoteSuggestionID.ToString()
                };
                output.Add(suggestion);
            }
            return output;
        }

        public UserModel getLoggedInUserIfValid(string DisplayName, string Password)
        {
            var tamp = _connection.QueryFirstOrDefault("CALL GetLoggedInUserIfValid(@nDisplayName, @nPassword)",
                    new
                    {
                        @nDisplayName = DisplayName,
                        @nPassword = Password
                    }
                );
            UserModel user = new();
            if (tamp is not null){
                user = new()
                {
                    Id = tamp.UserID.ToString(),
                    FirstName = tamp.Fname,
                    LastName = tamp.LName,
                    DisplayName = tamp.DisplayName,
                    EmailAddress = tamp.Email,
                    AuthoredSuggestion = this.getAuthoredSuggestionsByID(tamp.UserID.ToString()),
                    VotedOnSuggestions = this.getUpvotesByUserID(tamp.UserID.ToString())
                };
            }
            _cache.Set(CacheName, user, TimeSpan.FromDays(value: 1));
            return user;
        }

        public async Task<UserModel> getLoggedInUserIfValidAsync(string DisplayName, string Password)
        {
            var tamp = await _connection.QueryFirstOrDefaultAsync("CALL GetLoggedInUserIfValid(@nDisplayName, @nPassword)",
                    new
                    {
                        @nDisplayName = DisplayName,
                        @nPassword = Password
                    }
                );
            UserModel user = new();
            if (tamp is not null)
            {
                user = new()
                {
                    Id = Convert.ToString(tamp.UserID) ,
                    FirstName = tamp.Fname,
                    LastName = tamp.LName,
                    DisplayName = tamp.DisplayName,
                    EmailAddress = tamp.Email,
                    AuthoredSuggestion = this.getAuthoredSuggestionsByID(Convert.ToString(tamp.UserID)),
                    VotedOnSuggestions = this.getUpvotesByUserID(Convert.ToString(tamp.UserID))
                };
            }
            _cache.Set(CacheName, user, TimeSpan.FromDays(value: 1));
            return user;
        }

        public async Task<bool> getAdminLevelFromDisplayName(string DisplayName)
        {
            var tamp = await _connection.QueryFirstOrDefaultAsync("CALL GetAdminLevelFromDisplayName(@nDisplayName)",
                    new
                    {
                        @nDisplayName = DisplayName
                    }
                );
            if(tamp.isAdmin == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }

    
}
