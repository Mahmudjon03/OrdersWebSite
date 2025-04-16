using RestSharp;
using System.Threading.Tasks;
using RestaurantLibrary.Database;
using RestaurantLibrary.Helpers.Enums;

namespace RestaurantLibrary.Models
{
    public class License
    {
        public int license_id { get; set; }
        public string license_key { get; set; }
        public EnumLicenseType license_type { get; set; }
        public string license_last_updated { get; set; }
        public string license_expiry_date { get; set; }
        public string license_value { get; set; }

        private static RestClient client = new RestClient(DataSQL.URL + @"/license");

        public async Task<bool> OnInsertAsync(License license)
        {
            RestRequest req = new RestRequest("/insert_license.php")
                .AddParameter("license_key", license.license_key)
                .AddParameter("license_last_updated", license.license_last_updated)
                .AddParameter("license_expiry_date", license.license_expiry_date)
                .AddParameter("license_type", (int)license.license_type)
                .AddParameter("license_value", license.license_value);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<License> OnSelectAsync()
        {
            var req = new RestRequest("/select_license.php");

            var res = await client.GetAsync(req); // Используем асинхронный метод

            var source = DataSQL.Deserialize<License>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> DeleteLicense()
        {
            var req = new RestRequest("/clear_license.php");

            var res = await client.GetAsync(req);

            return res.IsSuccessful;
        }

        public async Task OnUpdateAsync(License updatedLicense)
        {
            await DeleteLicense();
            await OnInsertAsync(updatedLicense);
        }
    }
}
