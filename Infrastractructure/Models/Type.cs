using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Type
    {
        public int type_id { get; set; }
        public string type_name { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/type");

        public async Task<List<Type>> OnLoadAsync()
        {
            var req = new RestRequest("/load_type.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Type>(res.Content);

            return source;
        }
    }
}
