using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class StockCategory
    {
        public int st_cat_id { get; set; }
        public string st_cat_name { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/stock_category");

        public async Task<List<StockCategory>> OnLoadAsync()
        {
            var req = new RestRequest("/load_stock_category.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<StockCategory>(res.Content);

            return source;
        }

        public async Task<bool> OnDeleteAsync(StockCategory stockCategory, int userId)
        {
            var req = new RestRequest("/delete_stock_category.php")
                .AddParameter("st_cat_id", stockCategory.st_cat_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Удалил категорию товара: {stockCategory.st_cat_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdateAsync(StockCategory stockCategory, int userId)
        {
            var req = new RestRequest("/update_stock_category.php")
                .AddParameter("st_cat_name", stockCategory.st_cat_name)
                .AddParameter("st_cat_id", stockCategory.st_cat_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Изменил категорию товара:\nНаименование:{stockCategory.st_cat_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnInsertAsync(StockCategory stockCategory, int userId)
        {
            var req = new RestRequest("/insert_stock_category.php")
                .AddParameter("st_cat_name", stockCategory.st_cat_name);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Добавил новую категорию товара:\nНаименование:{stockCategory.st_cat_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }
    }
}
