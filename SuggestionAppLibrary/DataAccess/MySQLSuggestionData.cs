using Dapper;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
namespace SuggestionAppLibrary.DataAccess
{
    public class MySQLSuggestionData : ISuggestionData
    {
        private readonly IUserData _userData;
        private readonly ICategoryData _categoryData;
        private readonly IStatusData _statusData;
        private readonly IMemoryCache _cache;
        private readonly List<SuggestionModel> _suggestions;
        private const string CacheName = "SuggestionData";
        private readonly MySqlConnection _connection;
        public MySQLSuggestionData(IUserData userData, ICategoryData categoryData, IStatusData statusData, IMemoryCache cache, IDbConnection conn)
        {
            _connection = conn.connection;
            _userData = userData;
            _categoryData = categoryData;
            _statusData = statusData;
            _cache = cache;
            _suggestions = GetSuggestions();
        }

        /*
         SuggestionModel sugg = new()
                    {
                        Id = temp.SuggestionID,
                        Suggestion = temp.Suggestion,
                        Description = temp.Description,
                        DateCreated = temp.DateCreated,
                        Category = categoryList.FirstOrDefault(x => x.Id == temp.SuggestionCategoryID),
                        Author = userList.FirstOrDefault(x => x.Id == temp.AuthorID),
                        UserVotes = temp.UserVotes,
                        SuggestionStatus = statusList.FirstOrDefault(x => x.Id == temp.SuggestionStatusID),
                        OwnerNotes = temp.OwnerNotes,
                        ApprovedForRelease = ((temp.ApprovedForRelease == 0) ? false : true),
                        Archived = ((temp.Archived == 0) ? false : true),
                        Rejected = ((temp.Rejected == 0) ? false : true)
                    };
         */

        public async Task<List<SuggestionModel>> GetSuggestionsAsync()
        {
            var output = _cache.Get<List<SuggestionModel>>(CacheName);
            List<CategoryModel> categoryList = await _categoryData.GetAllCategoriesAsync();
            List<StatusModel> statusList = await _statusData.getAllStatusesAsync();
            List<BasicUserModel> userList = await _userData.GetBasicUsersAsync();

            if (output == null)
            {
                var suggestions = await _connection.QueryAsync("CALL GetSuggestions()");
                
                foreach (var temp in suggestions)
                {
                    String categoryID = temp.SuggestionCategoryID;
                    String statusID = temp.SuggestionStatusID;
                    SuggestionModel sugg = new()
                    {
                        Id = temp.SuggestionID.ToString(),
                        Suggestion = temp.Suggestion,
                        Description = temp.Description,
                        DateCreated = temp.DateCreated,
                        Category = categoryList.FirstOrDefault(x => x.Id == temp.SuggestionCategoryID),
                        Author = userList.FirstOrDefault(x => x.Id == temp.AuthorID),
                        UserVotes = temp.UserVotes,
                        SuggestionStatus = statusList.FirstOrDefault(x => x.Id == temp.SuggestionStatusID),
                        OwnerNotes = temp.OwnerNotes,
                        ApprovedForRelease = temp.ApprovedForRelease,
                        Archived = temp.Archived,
                        Rejected = temp.Rejected
                    };
                    output.Add(sugg);
                }
                _cache.Set(CacheName, output, TimeSpan.FromMinutes(value: 1));
            }
            return output;
        }

        public async Task<List<SuggestionModel>> GetUsersSuggestionsAsync(string userID) {
            var output = _cache.Get<List<SuggestionModel>>(userID);//name of cache is userID
            List<CategoryModel> categoryList = await _categoryData.GetAllCategoriesAsync();
            List<StatusModel> statusList = await _statusData.getAllStatusesAsync();
            List<BasicUserModel> userList = await _userData.GetBasicUsersAsync();
            if (output == null) {
                output = new List<SuggestionModel>();
                var suggestions = await _connection.QueryAsync("CALL GetUsersSuggestions( @Id)",
                    new {
                        @Id = Int32.Parse(userID),
                    }
                );
                foreach (var temp in suggestions) {
                     SuggestionModel suggestionModel = new SuggestionModel() {
                         Id = temp.SuggestionID.ToString(),
                         Suggestion = temp.Suggestion,
                         Description = temp.Description,
                         DateCreated = temp.DateCreated,
                         Category = categoryList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionCategoryID?.ToString())),
                         Author = userList.FirstOrDefault(x => x.Id.Equals(temp.AuthorID?.ToString())),
                         UserVotes = temp.UserVotes,
                         SuggestionStatus = statusList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionStatusID?.ToString())),
                         OwnerNotes = temp.OwnerNotes,
                         ApprovedForRelease = temp.ApprovedForRelease,
                         Archived = temp.Archived,
                         Rejected = temp.Rejected
                     };
                    output.Add(suggestionModel);
                }
                _cache.Set(userID, output, TimeSpan.FromMinutes(value: 1));
            }
            return output;
            

        }

        public List<SuggestionModel> GetSuggestions()
        {
            List<CategoryModel> categoryList = _categoryData.GetAllCategories();
            List<StatusModel> statusList = _statusData.getAllStatuses();
            List<BasicUserModel> userList = _userData.GetBasicUsers();
            var output = _cache.Get<List<SuggestionModel>>(CacheName);


            if (output == null)
            {
                output = new List<SuggestionModel>();
                var suggestions = _connection.Query("CALL GetSuggestions()");
                foreach (var temp in suggestions)
                {
                    SuggestionModel sugg = new()
                    {
                        Id = temp.SuggestionID.ToString(),
                        Suggestion = temp.Suggestion,
                        Description = temp.Description,
                        DateCreated = temp.DateCreated,
                        Category = categoryList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionCategoryID?.ToString())),
                        Author = userList.FirstOrDefault(x => x.Id.Equals(temp.AuthorID?.ToString())),
                        UserVotes = temp.UserVotes,
                        SuggestionStatus = statusList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionStatusID?.ToString())),
                        OwnerNotes = temp.OwnerNotes,
                        ApprovedForRelease = temp.ApprovedForRelease,
                        Archived = temp.Archived,
                        Rejected = temp.Rejected
                    };
                    output.Add(sugg);
                }
                _cache.Set(CacheName, output, TimeSpan.FromMinutes(value: 1));
            }
            return output;
        }

        public async Task<List<SuggestionModel>> GetAllSuggestions()
        {

            List<CategoryModel> categoryList = await _categoryData.GetAllCategoriesAsync();
            List<StatusModel> statusList = await _statusData.getAllStatusesAsync();
            List<BasicUserModel> userList = await _userData.GetBasicUsersAsync();

            var output = new List<SuggestionModel>();
            var suggestions = await _connection.QueryAsync("CALL GetAllSuggestions()");
            foreach (var temp in suggestions)
            {
                SuggestionModel sugg = new()
                {
                    Id = temp.SuggestionID.ToString(),
                    Suggestion = temp.Suggestion,
                    Description = temp.Description,
                    DateCreated = temp.DateCreated,
                    Category = categoryList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionCategoryID?.ToString())),
                    Author = userList.FirstOrDefault(x => x.Id.Equals(temp.AuthorID?.ToString())),
                    UserVotes = temp.UserVotes,
                    SuggestionStatus = statusList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionStatusID?.ToString())),
                    OwnerNotes = temp.OwnerNotes,
                    ApprovedForRelease = temp.ApprovedForRelease,
                    Archived = temp.Archived,
                    Rejected = temp.Rejected
                };
                output.Add(sugg);

            }
            return output;
        }

        public async Task<List<SuggestionModel>> GetAllApprovedSuggestionsAsync()
        {
            var output = _cache.Get<List<SuggestionModel>>(CacheName);
            List<CategoryModel> categoryList = await _categoryData.GetAllCategoriesAsync();
            List<StatusModel> statusList = await _statusData.getAllStatusesAsync();
            List<BasicUserModel> userList = await _userData.GetBasicUsersAsync();

            if (output == null)
            {
                output = new List<SuggestionModel>();
                var suggestions =  await _connection.QueryAsync("CALL GetAllApprovedSuggestions()");
                foreach (var temp in suggestions)
                {
                    SuggestionModel sugg = new()
                    {
                        Id = temp.SuggestionID.ToString(),
                        Suggestion = temp.Suggestion,
                        Description = temp.Description,
                        DateCreated = temp.DateCreated,
                        Category = categoryList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionCategoryID?.ToString())),
                        Author = userList.FirstOrDefault(x => x.Id.Equals(temp.AuthorID?.ToString())),
                        UserVotes = temp.UserVotes,
                        SuggestionStatus = statusList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionStatusID?.ToString())),
                        OwnerNotes = temp.OwnerNotes,
                        ApprovedForRelease = temp.ApprovedForRelease,
                        Archived = temp.Archived,
                        Rejected = temp.Rejected
                    };
                    output.Add(sugg);
                }
                _cache.Set(CacheName, output, TimeSpan.FromMinutes(value: 1));
            }

            return output;
        }

        public async Task<SuggestionModel> GetSuggestionAsync(string nID)
        {
            List<CategoryModel> categoryList = await _categoryData.GetAllCategoriesAsync();
            List<StatusModel> statusList = await _statusData.getAllStatusesAsync();
            List<BasicUserModel> userList = await _userData.GetBasicUsersAsync();
            var suggestions/*not really*/ = await _connection.QueryAsync("CALL GetSuggestionByID( @Id)",
                new
                {
                    @Id = nID
                }
            );
            SuggestionModel output = new SuggestionModel();
            foreach(var temp in suggestions)
            {
                output = new()
                {
                    Id = temp.SuggestionID.ToString(),
                    Suggestion = temp.Suggestion,
                    Description = temp.Description,
                    DateCreated = temp.DateCreated,
                    Category = categoryList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionCategoryID?.ToString())),
                    Author = userList.FirstOrDefault(x => x.Id.Equals(temp.AuthorID?.ToString())),
                    UserVotes = temp.UserVotes,
                    SuggestionStatus = statusList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionStatusID?.ToString())),
                    OwnerNotes = temp.OwnerNotes,
                    ApprovedForRelease = temp.ApprovedForRelease,
                    Archived = temp.Archived,
                    Rejected = temp.Rejected
                };
            }

            return output;
        }

        public async Task<List<SuggestionModel>> GetAllSuggestionsWaitingForApprovalAsync()
        {
            List<CategoryModel> categoryList = await _categoryData.GetAllCategoriesAsync();
            List<StatusModel> statusList = await _statusData.getAllStatusesAsync();
            List<BasicUserModel> userList = await _userData.GetBasicUsersAsync();
            var output = _cache.Get<List<SuggestionModel>>(CacheName);

            if (output == null)
            {
                output = new List<SuggestionModel>();
                var tempOutput = await _connection.QueryAsync("CALL GetAllSuggestionsWaitingForApproval()");
                foreach(var temp in tempOutput)
                {
                    SuggestionModel sugg = new()
                    {
                        Id = temp.SuggestionID.ToString(),
                        Suggestion = temp.Suggestion,
                        Description = temp.Description,
                        DateCreated = temp.DateCreated,
                        Category = categoryList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionCategoryID?.ToString())),
                        Author = userList.FirstOrDefault(x => x.Id.Equals(temp.AuthorID?.ToString())),
                        UserVotes = temp.UserVotes,
                        SuggestionStatus = statusList.FirstOrDefault(x => x.Id.Equals(temp.SuggestionStatusID?.ToString())),
                        OwnerNotes = temp.OwnerNotes,
                        ApprovedForRelease = temp.ApprovedForRelease,
                        Archived = temp.Archived,
                        Rejected = temp.Rejected
                    };
                    output.Add(sugg);
                }
                _cache.Set(CacheName, output, TimeSpan.FromMinutes(value: 1));
            }

            return output;
        }

        public async Task UpdateSuggestionAsync(SuggestionModel suggestion)
        {
                await _connection.ExecuteAsync("Call updateSuggestion(@nSuggestion, @nDescription, @nDateCreated, @nSuggestionCategoryID, @nAuthorID, @nOwnerNotes, @nSuggestionStatusID, @nApprovedForRelease, @nArchived, @nRejected)",
                new
                {
                    @nSuggestion = suggestion.Suggestion,
                    @nDescription = suggestion.Description,
                    @nDateCreated = suggestion.DateCreated,
                    @nSuggestionCategoryID = suggestion.Category.Id,
                    @nAuthorID = suggestion.Author.Id,
                    @nOwnerNotes = suggestion.OwnerNotes,
                    @nSuggestionStatusID = suggestion.SuggestionStatus?.Id,
                    @nApprovedForRelease = suggestion.ApprovedForRelease,
                    @nArchived = suggestion.Archived,
                    @nRejected = suggestion.Rejected,
                }
                );
            _cache.Remove(CacheName);
        }

        public async Task UpvoteSuggestionAsync(string suggestionId, string userId)
        {
                await _connection.ExecuteAsync("Call UpvoteSuggestion(@nSuggestionID, @nUserID)",
                    new
                    {
                        @nSuggestionID = Int32.Parse(suggestionId),
                        @nUserID = Int32.Parse(userId)
                    }
                );
        }

        public async Task createSuggestionAsync(SuggestionModel suggestion)
        {
            await _connection.ExecuteAsync("CALL InsertSuggestion(@nSuggestion, @nDescription, @nDateCreated, @nSuggestionCategoryID, @nAuthorID, @nOwnerNotes, @nSuggestionStatusID, @nApprovedForRelease, @nArchived, @nRejected)",
                new
                {
                    @nSuggestion = suggestion.Suggestion,
                    @nDescription = suggestion.Description,
                    @nDateCreated = suggestion.DateCreated,
                    @nSuggestionCategoryID = suggestion.Category.Id,
                    @nAuthorID = suggestion.Author.Id,
                    @nOwnerNotes = suggestion.OwnerNotes,
                    @nSuggestionStatusID = suggestion.SuggestionStatus?.Id,
                    @nApprovedForRelease = suggestion.ApprovedForRelease,
                    @nArchived = suggestion.Archived,
                    @nRejected = suggestion.Rejected,
                    }
                );
        }
    }
}
