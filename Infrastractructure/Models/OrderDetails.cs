using RestaurantLibrary.Database;
using RestaurantLibrary.Helpers.Enums;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class OrderDetails
    {
        public int details_id { get; set; }
        public int details_prod { get; set; }
        public double details_count { get; set; }
        public int details_order { get; set; }
        public int details_status { get; set; }
        public int details_sub_order { get; set; }
        public string details_comment { get; set; }
        public Order order { get; set; }
        public Product product { get; set; }

        public string OrderName { get { return order?.OrderNum; } }
        public string ProductName { get { return product?.prod_name; } }
        public double ProductPrice { get { return product.prod_price; } }
        public string ProdTypeName { get { return product?.type?.type_name; } }
        public string DetailDate { get { return order.order_date; } }

        public string OnStatusText
        {
            get
            {
                string str = "";
                switch (details_status)
                {
                    case (int)EnumDetailsStatus.Accept: str = "Принят"; break;
                    case (int)EnumDetailsStatus.Edit: str = "На готовке"; break;
                    case (int)EnumDetailsStatus.New: str = "На готовке"; break;
                    case (int)EnumDetailsStatus.Ready: str = "Подано"; break;
                    case (int)EnumDetailsStatus.Return: str = "Возврат"; break;
                }

                return str;
            }
        }

        private static RestClient client = new RestClient(DataSQL.URL + @"/details");

        public async Task<List<OrderDetails>> OnSelectOrderDetailsAsync(int order_id)
        {
            var req = new RestRequest("/select_order_details.php")
                .AddParameter("details_order", order_id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<OrderDetails>(res.Content);

            return source;
        }

        public async Task<List<OrderDetails>> OnSelectOrderDetailsBatchAsync(List<int> orderIds)
        {
            var req = new RestRequest("/select_order_details_batch.php")
                .AddParameter("details_orders", string.Join(",", orderIds)); // Передаем ID через запятую

            var res = await client.PostAsync(req);

            return DataSQL.Deserialize<OrderDetails>(res.Content);
        }

        public async Task<List<OrderDetails>> OnLoadAsync()
        {
            var req = new RestRequest("/load_details.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<OrderDetails>(res.Content);

            return source;
        }
        public async Task<bool> OnInsertAsync(OrderDetails orderDetails)
        {
            var req = new RestRequest("/insert_order_details.php")
                .AddParameter("details_count", DataSQL.ConvertDouble(orderDetails.details_count))
                .AddParameter("details_prod", orderDetails.details_prod)
                .AddParameter("details_status", orderDetails.details_status)
                .AddParameter("details_comment", orderDetails.details_comment)
                .AddParameter("details_sub_order", orderDetails.details_sub_order)
                .AddParameter("details_order", orderDetails.details_order);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<List<OrderDetails>> OnSelectAllProductDetailsAsync(int product_id)
        {
            var req = new RestRequest("/select_product_details.php")
                .AddParameter("details_prod", product_id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<OrderDetails>(res.Content);

            return source;
        }

        public async Task<bool> OnDeleteDetailCascadeAsync(int details_id)
        {
            var req = new RestRequest("/delete_detail_cascade.php")
                .AddParameter("details_id", details_id);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }
    }
}
