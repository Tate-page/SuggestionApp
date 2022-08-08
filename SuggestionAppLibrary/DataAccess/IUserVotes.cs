namespace SuggestionAppLibrary.DataAccess
{
    public interface IUserVotes
    {
        Task<List<UserVotesModel>> GetUpvotesByUserIdAsync(string nID);
    }
}