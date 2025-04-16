using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Entry
    {
        public int entry_id { get; set; }
        public int entry_vendor { get; set; }
        public string entry_date { get; set; }
        public double entry_summa { get; set; }
        public double entry_paid { get; set; }
        public string entry_comment { get; set; }
        public int entry_user { get; set; }

        public User user { get; set; }
        public Vendor vendor { get; set; }

        public string GetComment { get { return string.IsNullOrEmpty(entry_comment) ? "-" : entry_comment; } }
        public double GetEntryDebt { get { return entry_summa - entry_paid; } }
        public string GetUserName { get { return user?.user_name; } }
        public string GetVendorName { get { return vendor == null ? "Неизвестный поставщик" : vendor.vendor_name; } }
        public string GetEntryName { get { return $"Приход №{entry_id}"; } }

        private static readonly RestClient client = new RestClient(DataSQL.URL + @"/entry");

        public async Task<List<Entry>> OnLoadAsync()
        {
            var req = new RestRequest("/load_entry.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Entry>(res.Content);

            return source;
        }

        public async Task<bool> OnInsertAsync(Entry entry, string vendorName, int userId)
        {
            RestRequest req = new RestRequest("/insert_entry.php")
                .AddParameter("entry_vendor", entry.entry_vendor)
                .AddParameter("entry_date", entry.entry_date)
                .AddParameter("entry_summa", entry.entry_summa)
                .AddParameter("entry_paid", entry.entry_paid)
                .AddParameter("entry_user", entry.entry_user)
                .AddParameter("entry_comment", entry.entry_comment);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"Приход был успешно принят:\nСумма: {entry.entry_summa}\nОплачено: {entry.entry_paid}\nДолг: {entry.GetEntryDebt}\nПоставщик: {vendorName}", userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdateAsync(Entry entry, int userId)
        {
            RestRequest req = new RestRequest("/update_entry.php")
                .AddParameter("entry_id", entry.entry_id)
                .AddParameter("entry_vendor", entry.entry_vendor)
                .AddParameter("entry_summa", entry.entry_summa)
                .AddParameter("entry_paid", entry.entry_paid)
                .AddParameter("entry_comment", entry.entry_comment);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"{entry.GetEntryName} был изменен:\nСумма: {entry.entry_summa}\nОплачено: {entry.entry_paid}\nДолг: {entry.GetEntryDebt}", userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnPaidUpdateAsync(Entry entry, double summa, int userId)
        {
            RestRequest req = new RestRequest("/update_pay_entry.php")
                .AddParameter("entry_id", entry.entry_id)
                .AddParameter("entry_paid", entry.entry_paid);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"{entry.GetEntryName} был оплачен:\nСумма оплаты: {summa}\nДолг: {entry.GetEntryDebt}", userId);
                return true;
            }
            return false;
        }

        public async Task<Entry> OnSelectLastAsync()
        {
            var req = new RestRequest("/select_last_entry.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Entry>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> OnDeleteAsync(Entry entry, int userId)
        {
            var req = new RestRequest("/delete_entry.php")
                .AddParameter("entry_id", entry.entry_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"Удалил {entry.GetEntryName}", userId);
                return true;
            }
            return false;
        }
    }
}
