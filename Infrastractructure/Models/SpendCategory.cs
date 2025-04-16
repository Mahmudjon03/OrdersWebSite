using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class SpendCategory
    {
        public int category_id { get; set; }
        public string category_name { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/spend_category");

        public async Task<List<SpendCategory>> OnLoadAsync()
        {
            var req = new RestRequest("/load_category.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<SpendCategory>(res.Content);

            return source;
        }

        public async Task<bool> OnUpdateAsync(SpendCategory category, int userId)
        {
            RestRequest req = new RestRequest("/update_category.php")
                .AddParameter("category_name", category.category_name)
                .AddParameter("category_id", category.category_id);

            var res = await client.PostAsync(req);

            if (!res.IsSuccessful)
            {
                return false;
            }
            else
            {
                var str = string.Format("Изменил категорию расхода\n" +
                    "Наименование: {0}\n",
                    category.category_name);
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
        }

        public async Task<bool> OnDeleteAsync(int id, int userId)
        {
            var req = new RestRequest("/delete_category.php")
                .AddParameter("category_id", id);

            var res = await client.PostAsync(req);

            if (!res.IsSuccessful)
            {
                return false;
            }
            else
            {
                await Debug.DebugInsertAsync("Удалил категорию расхода id: " + id, userId);
                return true;
            }
        }

        public async Task<bool> OnInsertAsync(SpendCategory category, int userId)
        {
            var req = new RestRequest("/insert_category.php")
                .AddParameter("category_name", category.category_name);

            var res = await client.PostAsync(req);

            if (!res.IsSuccessful)
            {
                return false;
            }
            else
            {
                var str = string.Format("Добавил новую категорию расхода\n" +
                    "Наименование: {0}\n",
                    category.category_name);

                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
        }

        public async Task<SpendCategory> OnSelectAsync(int id)
        {
            var req = new RestRequest("/select_category.php")
                .AddParameter("category_id", id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<SpendCategory>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }
    }
}