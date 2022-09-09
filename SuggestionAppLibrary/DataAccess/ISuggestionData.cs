namespace SuggestionAppLibrary.DataAccess
{
    public interface ISuggestionData
    {

        Task createSuggestionAsync(SuggestionModel suggestion);
        Task<List<SuggestionModel>> GetAllApprovedSuggestionsAsync();
        Task<List<SuggestionModel>> GetAllSuggestionsWaitingForApprovalAsync();
        Task<SuggestionModel> GetSuggestionAsync(string nID);
        List<SuggestionModel> GetSuggestions();
        Task<List<SuggestionModel>> GetSuggestionsAsync();
        Task<List<SuggestionModel>> GetUsersSuggestionsAsync(string userID);
        Task UpdateSuggestionAsync(SuggestionModel suggestion);
        Task UpvoteSuggestionAsync(string suggestionId, string userId);
        Task<List<UserVotesModel>> getUpvotesBySuggestionIDAsync(string id);
        List<UserVotesModel> getUpvotesBySuggestionID(string id);
    }
}