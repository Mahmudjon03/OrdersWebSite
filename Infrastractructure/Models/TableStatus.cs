using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class TableStatus
    {
        public int table_st_id { get; set; }
        public string table_st_name { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/table");

        public async Task<List<TableStatus>> OnLoadAsync()
        {
            var req = new RestRequest("/load_table_status.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<TableStatus>(res.Content);

            return source;
        }
    }
}
