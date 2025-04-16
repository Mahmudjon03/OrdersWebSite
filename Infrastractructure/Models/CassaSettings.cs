using System.Threading.Tasks;
using RestaurantLibrary.Database;
using RestSharp;

namespace RestaurantLibrary.Models
{
    public class CassaSettings
    {
        public int table_divide { get; set; }
        public int printer_settings { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/cassa_settings");

        public async Task<bool> OnUpdateTableDivideAsync(int tableDivide)
        {
            RestRequest req = new RestRequest("/update_table_divide.php")
                .AddParameter("table_divide", tableDivide);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnUpdatePrinterSettingsAsync(int printerSettings)
        {
            RestRequest req = new RestRequest("/update_printer_settings.php")
                .AddParameter("printer_settings", printerSettings);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> IsTablesDivisibleAsync()
        {
            var req = new RestRequest("/load_settings.php");
            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<CassaSettings>(res.Content);

            return source[0]?.table_divide == 1;
        }

        public async Task<bool> IsPrinterCookingOutputAsync()
        {
            var req = new RestRequest("/load_settings.php");
            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<CassaSettings>(res.Content);

            return source[0]?.printer_settings == 1;
        }
    }
}
