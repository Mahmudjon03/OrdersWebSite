using Newtonsoft.Json.Linq;
using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class DetailsProductComplex
    {
        public List<OrderDetails> OrderDetails { get; set; }
        public List<Product> Products { get; set; }
        public List<Type> ProdTypes { get; set; }

        private static readonly RestClient client = new RestClient(DataSQL.URL + @"/complex");

        public DetailsProductComplex DeserializeComplexData(string json, bool initProds)
        {
            DetailsProductComplex complexData = new DetailsProductComplex();
            JArray jsonArray = JArray.Parse(json);

            complexData.OrderDetails = jsonArray[0].ToObject<List<OrderDetails>>();
            complexData.Products = jsonArray[0].ToObject<List<Product>>();
            complexData.ProdTypes = jsonArray[0].ToObject<List<Type>>();

            if (initProds)
            {
                for (int i = 0; i < complexData.Products.Count; i++)
                {
                    complexData.Products[i].prod_total = complexData.OrderDetails[i].details_count;
                    //complexData.Products[i].prod_status = complexData.OrderDetails[i].details_status;
                    //complexData.Products[i].prod_sub_order = complexData.OrderDetails[i].details_sub_order;
                    //complexData.Products[i].type = complexData.ProdTypes[i];
                }
            }
            return complexData;
        }

        public async Task<DetailsProductComplex> OnSelectByKitchenAsync(int main_order_id, int order_sub, int kitchen_Id, bool initProds = false)
        {
            var req = new RestRequest("/load_details_products.php")
                .AddParameter("order_main", main_order_id)
                .AddParameter("order_sub", order_sub)
                .AddParameter("prod_kitchen", kitchen_Id);

            var res = await client.PostAsync(req);
            var source = DeserializeComplexData(res.Content, initProds);

            return source;
        }

        public async Task<DetailsProductComplex> OnLoadProdsAsync(int main_order_id, int order_sub, bool initProds = false)
        {
            var req = new RestRequest("/load_products.php")
                .AddParameter("order_main", main_order_id)
                .AddParameter("order_sub", order_sub);

            var res = await client.PostAsync(req);
            var source = DeserializeComplexData(res.Content, initProds);

            return source;
        }

        public async Task<DetailsProductComplex> OnLoadAllProdsAsync(int main_order_id, bool initProds = false)
        {
            var req = new RestRequest("/load_allproducts.php")
                .AddParameter("order_main", main_order_id);

            var res = await client.PostAsync(req);
            var source = DeserializeComplexData(res.Content, initProds);

            return source;
        }
    }
}
