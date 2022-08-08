using Dapper;
using Microsoft.Extensions.Caching.Memory;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace SuggestionAppLibrary.DataAccess
{
    public class MySQLCategoryData : ICategoryData
    {
        private List<CategoryModel> _categories;
        private readonly IMemoryCache _cache;
        private const string CacheName = "CategoryData";
        private readonly MySqlConnection _connection;
        public MySQLCategoryData(IMemoryCache cache, IDbConnection conn)
        {
            _connection = conn.connection;
            _cache = cache;
            _categories = GetAllCategories();
        }

        public List<CategoryModel> GetAllCategories()
        {
            var output = _cache.Get<List<CategoryModel>>(CacheName);

            if (output == null)
            {
                output = new List<CategoryModel>();
                var temp = _connection.Query("CALL GetAllCategories()");
                foreach (var item in temp)
                {
                    CategoryModel cat = new()
                    {
                        Id = item.CategoryID.ToString(),
                        CategoryName = item.CategoryName,
                        CategoryDescription = item.CategoryDescription
                    };
                    output.Add(cat);
                }

                _cache.Set(CacheName, output, TimeSpan.FromDays(value: 1));
            }

            return output;
        }

        public async Task<List<CategoryModel>> GetAllCategoriesAsync()
        {
            var output = _cache.Get<List<CategoryModel>>(CacheName);

            if (output == null)
            {
                output = new List<CategoryModel>();
                var temp = await _connection.QueryAsync("CALL GetAllCategories()");
                foreach(var item in temp)
                {
                    CategoryModel cat = new()
                    {
                        Id = item.CategoryID.ToString(),
                        CategoryName = item.CategoryName,
                        CategoryDescription = item.CategoryDescription
                    };
                    output.Add(cat);
                }

                _cache.Set(CacheName, output, TimeSpan.FromDays(value: 1));
            }

            return output;
        }

        public async Task CreateCategoryAsync(CategoryModel category)
        {
            await _connection.ExecuteAsync("CALL InsertCategory(@nCategoryName, @nCategoryDescription)",
                new
                {
                    @nCategoryName = category.CategoryName,
                    @nCategoryDescription = category.CategoryDescription
                }
            );
        }
    }
}
