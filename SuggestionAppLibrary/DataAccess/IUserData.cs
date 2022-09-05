namespace SuggestionAppLibrary.DataAccess
{
    public interface IUserData
    {
        Task<List<UserModel>> GetUsersAsync();
        Task<UserModel> GetUserFromIdAsync(string id);
        Task<UserModel> GetLastCreatedUserAsync();
        Task CreateUserAsync(UserModel user, string password, string favoriteColor);
        Task updateUserAsync(UserModel user);
        Task<List<BasicSuggestionModel>> getAuthoredSuggestionsByIDAsync(string id);
        List<BasicSuggestionModel> getAuthoredSuggestionsByID(string id);
        Task<List<UserVotesModel>> getUpvotesByUserIDAsync(string id);
        List<UserVotesModel> getUpvotesByUserID(string id);
        Task<List<BasicUserModel>> GetBasicUsersAsync();
        List<BasicUserModel> GetBasicUsers();
        void CreateUser(UserModel user, string password, string favoriteColor);

        UserModel getLoggedInUserIfValid(string DisplayName, string Password);
    }
}