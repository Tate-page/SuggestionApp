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
        public bool isAdmin { get; set; }
        public UserModel returnAsUserModel()
        {
            UserModel user = new()
            {
                Id = this.Id,
                FirstName = this.FirstName,
                LastName = this.LastName,
                DisplayName = this.DisplayName,
                EmailAddress = this.EmailAddress,
                AuthoredSuggestion = this.AuthoredSuggestion,
                VotedOnSuggestions = this.VotedOnSuggestions,
            };
            return user;
        }

        public void Clear()
        {
            this.IsLoggedIn = false;
            this.Id = null;
            this.FirstName = null;
            this.LastName = null;
            this.DisplayName = null;
            this.EmailAddress = null;
            this.AuthoredSuggestion.Clear();
            this.VotedOnSuggestions.Clear();
            this.isAdmin = false;
        }
    }
}
