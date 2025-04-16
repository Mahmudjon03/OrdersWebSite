using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Recipe
    {
        public int recipe_id { get; set; }
        public int recipe_stock { get; set; }
        public int recipe_product { get; set; }
        public double recipe_count { get; set; }
        public int recipe_value { get; set; }

        public Type type { get; set; }
        public Stock stock { get; set; }
        public Product product { get; set; }
        public string TypeName
        {
            get { return type?.type_name ?? "-"; }
        }

        public string StockName
        {
            get { return stock?.stock_name ?? "-"; }
        }

        public string ProductName
        {
            get { return product?.prod_name ?? "-"; }
        }

        private static RestClient client = new RestClient(DataSQL.URL + @"/recipe");

        public async Task<List<Recipe>> OnSelectProdRecipeAsync(int id)
        {
            var req = new RestRequest("/select_product_recipe.php")
                .AddParameter("recipe_product", id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Recipe>(res.Content);

            return source;
        }

        public async Task<List<Recipe>> OnSelectStockInRecipeAsync(int id)
        {
            var req = new RestRequest("/select_stock_recipe.php")
                .AddParameter("recipe_stock", id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Recipe>(res.Content);

            return source;
        }

        public async Task<Recipe> OnSelectAsync(int stock_id, int prod_id)
        {
            var req = new RestRequest("/select_recipe.php")
                .AddParameter("recipe_stock", stock_id)
                .AddParameter("recipe_product", prod_id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Recipe>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<List<Recipe>> OnSelectProductAsync(int id)
        {
            var req = new RestRequest("/select_product_recipe.php")
                .AddParameter("recipe_product", id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Recipe>(res.Content);

            return source;
        }

        public async Task<bool> OnDeleteProdAsync(int id)
        {
            var req = new RestRequest("/delete_recipe_prod.php")
                .AddParameter("recipe_product", id);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnInsertAsync(Recipe recipe)
        {
            var req = new RestRequest("/insert_recipe.php")
                .AddParameter("recipe_stock", recipe.recipe_stock)
                .AddParameter("recipe_product", recipe.recipe_product)
                .AddParameter("recipe_count", DataSQL.ConvertDouble(recipe.recipe_count))
                .AddParameter("recipe_value", recipe.recipe_value);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

    }
}
