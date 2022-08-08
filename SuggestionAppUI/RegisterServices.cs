namespace SuggestionAppUI
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder) {
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMemoryCache();

            builder.Services.AddSingleton<IDbConnection, DbConnection>();
            builder.Services.AddSingleton<ICategoryData, MySQLCategoryData>();
            builder.Services.AddSingleton<IStatusData, MySQLStatusData>();
            builder.Services.AddSingleton<ISuggestionData, MySQLSuggestionData>();
            builder.Services.AddSingleton<IUserData, MySQLUserData>();
            builder.Services.AddSingleton<IUserVotes, MySQLUserVotes>();
        }
    }
}
