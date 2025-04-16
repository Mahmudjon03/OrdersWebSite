using RestaurantLibrary.Database;
using RestSharp;

namespace RestaurantLibrary.Models
{
    public class Category
    {
        public int category_id { get; set; }
        public string category_name { get; set; }
        public int category_stock { get; set; }
        public int category_type { get; set; }
        public StockCategory st_category { get; set; }
        public Type type { get; set; }
        public List<Product> product { get; set; }
        public string TypeName { get { return type?.type_name; } }
        public string StockCategoryName { get { return st_category?.st_cat_name; } }

        private static RestClient client = new RestClient(DataSQL.URL + @"/category");

        public async Task<List<Category>> OnLoadAsync()
        {
            var req = new RestRequest("/load_category.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Category>(res.Content);

            return source;
        }

        public async Task<List<Category>> OnLoadAsync(int stock_category)
        {
            var req = new RestRequest("/load_category_type.php")
                .AddParameter("category_stock", stock_category);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Category>(res.Content);

            return source;
        }

        public async Task<Category> OnSelectLastAsync()
        {
            var req = new RestRequest("/select_last_category.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Category>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> OnUpdateAsync(Category updated, int userId)
        {
            RestRequest req = new RestRequest("/update_category.php")
                .AddParameter("category_name", updated.category_name)
                .AddParameter("category_type", updated.category_type)
                .AddParameter("category_stock", updated.category_stock)
                .AddParameter("category_id", updated.category_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Изменил данные кухни:\nНаименование: {updated.category_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnDeleteAsync(Category deleted, int userId)
        {
            var req = new RestRequest("/delete_category.php")
                .AddParameter("category_id", deleted.category_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"Удалил категорию: {deleted.category_name}", userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnInsertAsync(Category category, int userId)
        {
            var req = new RestRequest("/insert_category.php")
                .AddParameter("category_name", category.category_name)
                .AddParameter("category_stock", category.category_stock)
                .AddParameter("category_type", category.category_type);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Добавил новую категорию:\nНаименование: {category.category_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }
    }
}