using RestaurantLibrary.Database;
using RestaurantLibrary.Helpers.Enums;
using RestSharp;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Payment
    {
        public double payment_percent { get; set; }
        public double payment_fix { get; set; }
        public int payment_type { get; set; }

        public async Task<double> OnGetPercentAsync()
        {
            var payment = await OnLoadAsync();
            return payment.payment_percent;
        }

        public async Task<double> OnGetFixAsync()
        {
            var payment = await OnLoadAsync();
            return payment.payment_fix;
        }

        private static RestClient client = new RestClient(DataSQL.URL + @"/payment");

        public async Task<Payment> OnLoadAsync()
        {
            var req = new RestRequest("/load_payment.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Payment>(res.Content);

            return source[0];
        }

        public async Task<bool> OnUpdateAsync(Payment payment, int userId)
        {
            RestRequest req = new RestRequest("/update_payment.php")
                .AddParameter("payment_percent", DataSQL.ConvertDouble(payment.payment_percent))
                .AddParameter("payment_fix", DataSQL.ConvertDouble(payment.payment_fix))
                .AddParameter("payment_type", DataSQL.ConvertDouble(payment.payment_type));

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                string type;
                string value;
                if (payment.payment_type == (int)EnumCasheType.Fix)
                {
                    type = "Фиксированная ставка";
                    value = $"Фикс.оплата: {payment.payment_fix}";
                }
                else
                {
                    type = "Процентная ставка";
                    value = $"Процент официанту: {payment.payment_percent}";
                }
                await Debug.DebugInsertAsync($"Изменил настройку оплаты официантам:\nТип:{type}\n{value}", userId);
                return true;
            }
            return false;
        }
    }
}