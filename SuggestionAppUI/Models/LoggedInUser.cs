namespace SuggestionAppUI.Models
{
    public class LoggedInUser : ILoggedInUser
    {
        public Boolean IsLoggedIn { get; set; }
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string EmailAddress { get; set; }
        public List<BasicSuggestionModel> AuthoredSuggestion { get; set; } = new();
        public List<UserVotesModel> VotedOnSuggestions { get; set; } = new();
    }
}
