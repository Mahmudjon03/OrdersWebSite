using System;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantLibrary.Database;

namespace RestaurantLibrary.Models
{
    public class Stock
    {
        public int stock_id { get; set; }
        public string stock_name { get; set; }
        public double stock_count { get; set; }
        public double stock_price { get; set; }
        public int stock_value { get; set; }
        public int stock_category { get; set; }

        public Type type { get; set; }
        public StockCategory stockCategory { get; set; }

        public string StockCategoryName { get { return stockCategory?.st_cat_name; } }
        public string StockTypeName { get { return type?.type_name; } }
        public double Sum { get { return Math.Round(stock_count * stock_price, 2); } }

        private static RestClient client = new RestClient(DataSQL.URL + @"/stock");

        public async Task<List<Stock>> OnLoadAsync()
        {
            var req = new RestRequest("/load_stock.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Stock>(res.Content);

            return source;
        }

        public async Task<Stock> OnSelectLastAsync()
        {
            var req = new RestRequest("/select_last_stock.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Stock>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<Stock> OnSelectAsync(int stock_id)
        {
            var req = new RestRequest("/select_stock.php")
                .AddParameter("stock_id", stock_id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Stock>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> OnInsertAsync(Stock stock, int userId)
        {
            var req = new RestRequest("/insert_stock.php")
                .AddParameter("stock_name", stock.stock_name)
                .AddParameter("stock_price", DataSQL.ConvertDouble(stock.stock_price))
                .AddParameter("stock_count", DataSQL.ConvertDouble(stock.stock_count))
                .AddParameter("stock_category", stock.stock_category)
                .AddParameter("stock_value", stock.stock_value);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Создал товар на складе:\nНаименование: {stock.stock_name}\nСтоимость: {stock.stock_price}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdateAsync(Stock stock)
        {
            var req = new RestRequest("/update_stock.php")
                .AddParameter("stock_name", stock.stock_name)
                .AddParameter("stock_id", stock.stock_id)
                .AddParameter("stock_price", DataSQL.ConvertDouble(stock.stock_price))
                .AddParameter("stock_count", DataSQL.ConvertDouble(stock.stock_count))
                .AddParameter("stock_category", stock.stock_category)
                .AddParameter("stock_value", stock.stock_value);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnUpdateCountAsync(Stock stock)
        {
            var req = new RestRequest("/update_stock_count.php")
                .AddParameter("stock_id", stock.stock_id)
                .AddParameter("stock_count", DataSQL.ConvertDouble(stock.stock_count))
                .AddParameter("stock_price", DataSQL.ConvertDouble(stock.stock_price));

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnDeleteAsync(Stock stock, int userId)
        {
            var req = new RestRequest("/delete_stock.php")
                .AddParameter("stock_id", stock.stock_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"Удалил товар со склада: {stock.stock_name}", userId);
                return true;
            }
            return false;
        }
    }
}
