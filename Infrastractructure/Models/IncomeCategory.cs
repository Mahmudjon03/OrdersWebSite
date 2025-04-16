using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace RestaurantLibrary.Models
{
    public class IncomeCategory
    {
        public int category_id { get; set; }
        public string category_name { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/income_category");

        public async Task<List<IncomeCategory>> OnLoadAsync()
        {
            var req = new RestRequest("/load_category.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<IncomeCategory>(res.Content);

            return source;
        }
       
        public async Task<bool> OnUpdateAsync(IncomeCategory category, int userId)
        {
            RestRequest req = new RestRequest("/update_category.php")
                .AddParameter("category_name", category.category_name)
                .AddParameter("category_id", category.category_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Изменил категорию дохода:\nНаименование: {category.category_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnDeleteAsync(IncomeCategory category, int userId)
        {
            var req = new RestRequest("/delete_category.php")
                .AddParameter("category_id", category.category_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Удалил категорию дохода: {category.category_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnInsertAsync(IncomeCategory category, int userId)
        {
            var req = new RestRequest("/insert_category.php")
                .AddParameter("category_name", category.category_name);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Добавил категорию дохода: {category.category_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }
    }
}