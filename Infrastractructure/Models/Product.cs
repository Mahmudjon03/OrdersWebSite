using RestaurantLibrary.Database;
using RestaurantLibrary.Helpers;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Product
    {
        public int prod_id { get; set; }
        public string prod_name { get; set; }
        public string prod_inglish_name { get {
            return TransliterationHelper.CyrillicToLatin(prod_name);
                    }
        }

        public bool cart_state { get; set; } = false;
        public double prod_price { get; set; }
        public int prod_category { get; set; }
        public int prod_kitchen { get; set; }
        public int prod_cooking_minutes { get; set; }
        public double prod_total { get; set; }
        public int prod_value { get; set; }
        public int prod_type { get; set; }
        public double prod_start_price { get; set; }

        public Kitchen kitchen { get; set; }
        public Category category { get; set; }
        public Type type { get; set; }
        public List<Recipe> recipe { get; set; }
        public List<OrderDetails> orderDetails { get; set; }

        public int ProdBuy
        {
            get
            {
                var total = 0;
                foreach (var detail in orderDetails)
                {
                    total += (int)detail.details_count;
                }
                return total;
            }
        }
        public string ProdCategoryName { get { return category != null ? category.category_name : "Подкупной"; } }
        public string ProdTypeName { get { return type?.type_name; } }
        public string ProdKitchenName { get { return kitchen?.kitchen_name; } }

        private static RestClient client = new RestClient(DataSQL.URL + @"/product");

        public async Task<Product> OnSelectAsync(int prod_id)
        {
            var req = new RestRequest("/select_product.php")
                .AddParameter("prod_id", prod_id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Product>(res.Content);

            if (source.Count > 0)
                return source[0];
            else
                return null;
        }

        public async Task<Product> OnSelectLastAsync()
        {
            var req = new RestRequest("/select_last_product.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Product>(res.Content);

            if (source.Count > 0)
                return source[0];
            else
                return null;
        }

        public async Task<List<Product>> OnLoadAsync()
        {
            var req = new RestRequest("/load_product.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Product>(res.Content);

            return source;
        }

        public async Task<bool> OnUpdateAsync(Product product, int userId)
        {
            RestRequest req = new RestRequest("/update_product.php")
                .AddParameter("prod_id", product.prod_id)
                .AddParameter("prod_name", product.prod_name)
                .AddParameter("prod_price", DataSQL.ConvertDouble(product.prod_price))
                .AddParameter("prod_cooking_minutes", product.prod_cooking_minutes)
                .AddParameter("prod_category", product.prod_category)
                .AddParameter("prod_type", product.prod_type)
                .AddParameter("prod_start_price", DataSQL.ConvertDouble(product.prod_start_price))
                .AddParameter("prod_kitchen", product.prod_kitchen)
                .AddParameter("prod_value", product.prod_value);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Изменил блюдо:\nНаименование: {product.prod_name}\nКатегория:{product.category?.category_name}\nСебестоимость:{product.prod_start_price}\nСтоимость: {product.prod_price}\nКухня: {product.kitchen?.kitchen_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdatePriceAsync(Product product)
        {
            RestRequest req = new RestRequest("/update_product_price.php")
                .AddParameter("prod_id", product.prod_id)
                .AddParameter("prod_price", DataSQL.ConvertDouble(product.prod_price));

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnInsertAsync(Product product, int userId)
        {
            var req = new RestRequest("/insert_product.php")
                .AddParameter("prod_name", product.prod_name)
                .AddParameter("prod_price", DataSQL.ConvertDouble(product.prod_price))
                .AddParameter("prod_cooking_minutes", product.prod_cooking_minutes)
                .AddParameter("prod_category", product.prod_category)
                .AddParameter("prod_type", product.prod_type)
                .AddParameter("prod_start_price", DataSQL.ConvertDouble(product.prod_start_price))
                .AddParameter("prod_kitchen", product.prod_kitchen)
                .AddParameter("prod_value", product.prod_value);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Добавил новое блюдо:\nНаименование: {product.prod_name}\nКатегория:{product.category?.category_name}\nСебестоимость:{product.prod_start_price}\nСтоимость: {product.prod_price}\nКухня: {product.kitchen?.kitchen_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnDeleteAsync(Product product, int userId)
        {
            var req = new RestRequest("/delete_product.php")
                .AddParameter("prod_id", product.prod_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync("Удалил блюдо: " + product.prod_name, userId);
                return true;
            }
            return false;
        }
    }
}