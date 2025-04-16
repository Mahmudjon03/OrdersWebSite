using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Hall
    {
        public int hall_id { get; set; }
        public string hall_name { get; set; }
        public double hall_price { get; set; }
        public int hall_bonus { get; set; }
        public int hall_type { get; set; }
        public HallType hallType { get; set; }
        public List<Tables> tables { get; set; }
        public string GetHallType { get { return hallType?.type_name; } }

        private static RestClient client = new RestClient(DataSQL.URL + @"/hall");

        public async Task<List<Hall>> OnLoadAllAsync()
        {
            var req = new RestRequest("/load_all.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Hall>(res.Content);

            return source;
        }

        public async Task<List<Hall>> OnLoadHallAsync()
        {
            var req = new RestRequest("/load_hall.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Hall>(res.Content);

            return source;
        }

        public async Task<List<Hall>> OnLoadDeliveriesAsync()
        {
            var req = new RestRequest("/load_delivery.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Hall>(res.Content);

            return source;
        }

        public async Task<bool> OnDeleteAsync(Hall hall, int userId)
        {
            var req = new RestRequest("/delete_hall.php")
                .AddParameter("hall_id", hall.hall_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"Удалил зал:\nНаименование:{hall.hall_name}", userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdateAsync(Hall hall, int userId)
        {
            RestRequest req = new RestRequest("/update_hall.php")
                .AddParameter("hall_name", hall.hall_name)
                .AddParameter("hall_price", DataSQL.ConvertDouble(hall.hall_price))
                .AddParameter("hall_type", hall.hall_type)
                .AddParameter("hall_bonus", hall.hall_bonus)
                .AddParameter("hall_id", hall.hall_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Изменил данные зала:\nНаименование: {hall.hall_name}\nСтоимость: {hall.hall_price}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnInsertAsync(Hall hall, string payType, int userId)
        {
            var req = new RestRequest("/insert_hall.php")
                .AddParameter("hall_price", DataSQL.ConvertDouble(hall.hall_price))
                .AddParameter("hall_name", hall.hall_name)
                .AddParameter("hall_type", hall.hall_type)
                .AddParameter("hall_bonus", hall.hall_bonus)
                .AddParameter("hall_id", hall.hall_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Добавил новый зал:\nНаименование: {hall.hall_name}\nТип оплаты: {payType}\nСтоимость: {hall.hall_price}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }
    }
}
