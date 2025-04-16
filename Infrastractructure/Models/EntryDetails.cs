using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class EntryDetails
    {
        public int details_id { get; set; }
        public int details_stock { get; set; }
        public int details_entry { get; set; }
        public double details_count { get; set; }
        public double details_price { get; set; }
        public int details_value { get; set; }

        public Type type { get; set; }
        public Stock stock { get; set; }
        public Entry entry { get; set; }

        public string TypeName { get { return type?.type_name; } }
        public string StockName { get { return stock?.stock_name; } }
        public string EntryName { get { return entry?.GetEntryName; } }

        private static RestClient client = new RestClient(DataSQL.URL + @"/entry_details");

        public async Task<List<EntryDetails>> OnSelectDetailsByEntryAsync(int entry_id)
        {
            var req = new RestRequest("/select_details_entry.php")
                .AddParameter("details_entry", entry_id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<EntryDetails>(res.Content);

            return source;
        }

        public async Task<List<EntryDetails>> OnSelectDetailsByStockAsync(int stock_id)
        {
            var req = new RestRequest("/select_details_stock.php")
                .AddParameter("details_stock", stock_id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<EntryDetails>(res.Content);

            return source;
        }

        public async Task<bool> OnDeleteDetailsAsync(int id)
        {
            var req = new RestRequest("/delete_details.php")
                .AddParameter("details_id", id);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnDeleteDetailsByEntryAsync(int entry_id)
        {
            var req = new RestRequest("/delete_details_entry.php")
                .AddParameter("details_entry", entry_id);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnInsertAsync(EntryDetails details)
        {
            var req = new RestRequest("/insert_details.php")
                .AddParameter("details_stock", details.details_stock)
                .AddParameter("details_entry", details.details_entry)
                .AddParameter("details_count", DataSQL.ConvertDouble(details.details_count))
                .AddParameter("details_price", DataSQL.ConvertDouble(details.details_price))
                .AddParameter("details_value", details.details_value);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }
    }
}
