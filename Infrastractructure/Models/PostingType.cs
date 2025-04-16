using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class PostingType
    {
        public int post_type_id { get; set; }
        public string post_type_name { get; set; }

        public List<Posting> posting { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/posting_type");

        public async Task<List<PostingType>> OnLoadAsync()
        {
            var req = new RestRequest("/load_posting_type.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<PostingType>(res.Content);

            return source;
        }
    }
}
