using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Cassa
    {
        public int cassa_id { get; set; }
        public string cassa_name { get; set; }
        public double cassa_money { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/cassa");

        public async Task<List<Cassa>> OnLoadAsync()
        {
            var req = new RestRequest("/load_cassa.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Cassa>(res.Content);

            return source;
        }

        public async Task<bool> OnInsertAsync(Cassa cassa, int userId)
        {
            RestRequest req = new RestRequest("/insert_cassa.php")
                .AddParameter("cassa_name", cassa.cassa_name)
                .AddParameter("cassa_money", cassa.cassa_money);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Добавил новую кассу: '{cassa.cassa_name}'";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdateAsync(Cassa updated)
        {
            RestRequest req = new RestRequest("/update_cassa.php")
                .AddParameter("cassa_id", updated.cassa_id)
                .AddParameter("cassa_name", updated.cassa_name)
                .AddParameter("cassa_money", updated.cassa_money);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<Cassa> OnSelectCassaAsync(int id)
        {
            var req = new RestRequest("/select_cassa.php")
                .AddParameter("cassa_id", id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Cassa>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> OnDeleteCassa(Cassa deleted, int userId)
        {
            var req = new RestRequest("/delete_cassa.php")
                .AddParameter("cassa_id", deleted.cassa_id);

            var res = client.Post(req);

            if (res.IsSuccessful)
            {
                var str = $"Удалил кассу: '{deleted.cassa_name}'";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return cassa_name;
        }
    }
}
