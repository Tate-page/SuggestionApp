namespace SuggestionAppLibrary.DataAccess
{
    public interface ICategoryData
    {
        Task CreateCategoryAsync(CategoryModel category);
        List<CategoryModel> GetAllCategories();

        Task<List<CategoryModel>> GetAllCategoriesAsync();
    }
}