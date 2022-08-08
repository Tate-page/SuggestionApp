namespace SuggestionAppLibrary.DataAccess
{
    public interface IStatusData
    {
        Task CreateStatusAsync(StatusModel status);
        List<StatusModel> getAllStatuses();

        Task<List<StatusModel>> getAllStatusesAsync();
    }
}