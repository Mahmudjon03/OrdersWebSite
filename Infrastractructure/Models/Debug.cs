using RestaurantLibrary.Database;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Debug
    {
        public int debug_id { get; set; }
        public int debug_admin { get; set; }
        public string debug_date { get; set; }
        public string debug_text { get; set; }

        public User user { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/debug");

        public static async Task DebugInsertAsync(string text, int admin = 0)
        {
            var req = new RestRequest("/insert_debug.php")
                .AddParameter("debug_admin", admin)
                .AddParameter("debug_date", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString())
                .AddParameter("debug_text", text);

            await client.PostAsync(req);
        }
    }
}
