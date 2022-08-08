﻿using Dapper;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.Data;

namespace SuggestionAppLibrary.DataAccess
{
    public class MySQLUserData : IUserData
    {
        private readonly MySqlConnection _connection;
        public MySQLUserData(IDbConnection conn)
        {
            _connection = conn.connection;
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

        public async Task CreateUserAsync(UserModel user)
        {
            await _connection.ExecuteAsync("CALL InsertUser(@nFname, @nLname, @DisplayName, @nEmail)",
                new
                {
                    @nFname = user.FirstName,
                    @nLname = user.LastName,
                    @DisplayName = user.DisplayName,
                    @nEmail = user.EmailAddress
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
                    UserID = item.SuggestionID.ToString(),
                    SuggestionID = item.Suggestion
                };
                output.Add(suggestion);
            }
            return output;
        }
    }
}
