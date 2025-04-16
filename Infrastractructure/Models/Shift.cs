using RestaurantLibrary.Database;
using RestaurantLibrary.Helpers.Enums;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Shift
    {
        public int shift_id { get; set; }
        public string shift_date_open { get; set; }
        public string shift_date_close { get; set; }
        public int shift_user_id { get; set; }
        public int shift_status { get; set; }

        public User user { get; set; }
        public List<Order> shiftOrders;

        public string GetUserName { get { return user?.user_name; } }
        public string OnStatus { get { return shift_status == (int)EnumShift.Close ? shift_date_close : "Смена открыта"; } }

        public int OrdersCount => shiftOrders?.Count ?? 0;
        public double OrdersSum => shiftOrders?.Sum(u => u.order_price) ?? 0;

        private static RestClient client = new RestClient(DataSQL.URL + @"/shift");

        public async Task<List<Shift>> OnLoadAsync()
        {
            var req = new RestRequest("/load_shift.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Shift>(res.Content);

            return source;
        }
    }
}
