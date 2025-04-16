using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Posting
    {
        public int post_id { get; set; }
        public int post_stock_id { get; set; }
        public double post_value { get; set; }
        public int post_type { get; set; }
        public string post_date { get; set; }
        public int post_categories { get; set; }

        public virtual Type type { get; set; }
        public virtual Stock stock { get; set; }
        public virtual PostingType posting_type { get; set; }

        public string GetTypeName { get { return type?.type_name; } }
        public string GetStockName { get { return stock?.stock_name; } }
        public string GetPostingType { get { return posting_type?.post_type_name; } }

        private static RestClient client = new RestClient(DataSQL.URL + @"/posting");

        public async Task<List<Posting>> OnLoadAsync()
        {
            var req = new RestRequest("/load_posting.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Posting>(res.Content);

            return source;
        }

        public async Task<bool> OnInsertAsync(Posting posting, int userId)
        {
            var req = new RestRequest("/insert_posting.php")
                .AddParameter("post_stock_id", posting.post_stock_id)
                .AddParameter("post_value", DataSQL.ConvertDouble(posting.post_value))
                .AddParameter("post_date", posting.post_date)
                .AddParameter("post_categories", posting.post_categories)
                .AddParameter("post_type", posting.post_type);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"Операция с товаром:\nТовар:{posting.stock.stock_name}\nТип операции:{posting.type.type_name}\nКоличество:{posting.post_value}", userId);
                return true;
            }
            return false;
        }

        public async Task<List<Posting>> OnSelectAsync(int id)
        {
            var req = new RestRequest("/select_posting.php")
                .AddParameter("post_stock_id", id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Posting>(res.Content);

            return source;
        }

    }
}
