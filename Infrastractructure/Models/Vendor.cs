using RestSharp;
using System.Threading.Tasks;
using RestaurantLibrary.Database;
using System.Collections.Generic;

namespace RestaurantLibrary.Models
{
    public class Vendor
    {
        public int vendor_id { get; set; }
        public string vendor_name { get; set; }
        public string vendor_phone { get; set; }
        public double vendor_debt { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/vendor");

        public async Task<List<Vendor>> OnLoadAsync()
        {
            var req = new RestRequest("/load_vendor.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Vendor>(res.Content);

            return source;
        }

        public async Task<List<Vendor>> OnLoadDebtorsAsync()
        {
            var req = new RestRequest("/load_debtors.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Vendor>(res.Content);

            return source;
        }

        public async Task<bool> OnInsertAsync(Vendor vendor, int userId)
        {
            var req = new RestRequest("/insert_vendor.php")
                .AddParameter("vendor_name", vendor.vendor_name)
                .AddParameter("vendor_phone", vendor.vendor_phone)
                .AddParameter("vendor_debt", vendor.vendor_debt);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Добавил новый поставщик:\nИмя: {vendor.vendor_name}";
                await Debug.DebugInsertAsync(str, userId);
                //await Debug.DebugInsertAsync(str, Settings.Default.user_id);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdateAsync(Vendor vendor, int userId)
        {
            var existing = await OnSelectAsync(vendor.vendor_id);
            var req = new RestRequest("/update_vendor.php")
                .AddParameter("vendor_id", vendor.vendor_id)
                .AddParameter("vendor_debt", vendor.vendor_debt);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Изменил данные поставщика:\nИмя: {vendor.vendor_name}\nДолг до: {existing?.vendor_debt}\nДолг после: {vendor.vendor_debt}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<Vendor> OnSelectAsync(int id)
        {
            var req = new RestRequest("/select_vendor.php")
                .AddParameter("vendor_id", id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Vendor>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<Vendor> OnSelectLastAsync()
        {
            var req = new RestRequest("/select_last_vendor.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Vendor>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> OnDeleteAsync(Vendor vendor, int userId)
        {
            var req = new RestRequest("/delete_vendor.php")
                .AddParameter("vendor_id", vendor.vendor_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = $"Удалил поставщика:\nИмя: {vendor.vendor_name}";
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }
    }
}
