namespace SuggestionAppLibrary.DataAccess
{
    public interface IUserData
    {
        Task<List<UserModel>> GetUsersAsync();
        Task<UserModel> GetUserFromIdAsync(string id);
        Task<UserModel> GetLastCreatedUserAsync();
        Task CreateUserAsync(UserModel user);
        Task updateUserAsync(UserModel user);
        Task<List<BasicSuggestionModel>> getAuthoredSuggestionsByIDAsync(string id);
        Task<List<UserVotesModel>> getUpvotesByUserIDAsync(string id);
        Task<List<BasicUserModel>> GetBasicUsersAsync();
        List<BasicUserModel> GetBasicUsers();


    }
}