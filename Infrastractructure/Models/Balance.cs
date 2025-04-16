using System.Threading.Tasks;
using RestaurantLibrary.Database;
using RestSharp;

namespace RestaurantLibrary.Models
{
    public class Balance
    {
        public int budget_id { get; set; }
        public string budget_name { get; set; }
        public double budget_money { get; set; }

        private static readonly RestClient client = new RestClient(DataSQL.URL + @"/budget");

        public async Task<double> OnGetBalanceAsync()
        {
            var req = new RestRequest("/load_budget.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Balance>(res.Content);

            return source.Count > 0 ? source[0].budget_money : 0;
        }

        public async Task<Balance> OnLoadAsync()
        {
            var req = new RestRequest("/load_budget.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Balance>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> OnUpdateBalance(double newBalance)
        {
            RestRequest req = new RestRequest("/update_budget.php")
                .AddParameter("budget_money", newBalance);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }
    }
}
