using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Client
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string client_address { get; set; }
        public string client_phone { get; set; }
        public double client_debt { get; set; }

        private static readonly RestClient client = new RestClient(DataSQL.URL + @"/client");

        public async Task<List<Client>> OnLoadAsync()
        {
            var req = new RestRequest("/load_client.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Client>(res.Content);

            return source;
        }

        public async Task<List<Client>> OnLoadDebtorsAsync()
        {
            var req = new RestRequest("/load_debtors.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Client>(res.Content);

            return source;
        }
    }
}
