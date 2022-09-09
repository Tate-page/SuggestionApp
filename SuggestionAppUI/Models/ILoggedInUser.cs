namespace SuggestionAppUI.Models
{
    public interface ILoggedInUser
    {
        Boolean IsLoggedIn { get; set; }
        List<BasicSuggestionModel> AuthoredSuggestion { get; set; }
        string DisplayName { get; set; }
        string EmailAddress { get; set; }
        string FirstName { get; set; }
        string Id { get; set; }
        string LastName { get; set; }
        List<UserVotesModel> VotedOnSuggestions { get; set; }
        bool isAdmin { get; set; }
        UserModel returnAsUserModel();
        void Clear();
    }
}