
using RestaurantLibrary.Database;
using RestaurantLibrary.Helpers.Enums;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models.Transactions
{
    public class OrderTransaction
    {
        public int transaction_id { get; set; }
        public string transaction_date { get; set; }
        public EnumPaymentType transaction_payment_type { get; set; }
        public int transaction_order { get; set; }
        public int transaction_cassa { get; set; }
        public int transaction_card { get; set; }
        public double transaction_cash_amount { get; set; }
        public double transaction_card_amount { get; set; }
        public double TotalAmount => transaction_cash_amount + transaction_card_amount;

        private static RestClient client = new RestClient(DataSQL.URL + @"/order_transactions");

        // Загрузка всех транзакций
        public async Task<List<OrderTransaction>> OnLoadAsync()
        {
            var req = new RestRequest("/load_transactions.php");
            var res = await client.GetAsync(req);
            return DataSQL.Deserialize<OrderTransaction>(res.Content);
        }

        // Создание новой транзакции
        public async Task<bool> OnInsertTransactionAsync()
        {
            var req = new RestRequest("/insert_order_transaction.php")
                .AddParameter("transaction_date", transaction_date)
                .AddParameter("transaction_payment_type", (int)transaction_payment_type)
                .AddParameter("transaction_order", transaction_order)
                .AddParameter("transaction_cassa", transaction_cassa)
                .AddParameter("transaction_card", transaction_card)
                .AddParameter("transaction_cash_amount", transaction_cash_amount)
                .AddParameter("transaction_card_amount", transaction_card_amount);

            var res = await client.PostAsync(req);
            return res.IsSuccessful;
        }

        // Обновление существующей транзакции
        public async Task<bool> OnUpdateTransactionAsync()
        {
            var req = new RestRequest("/update_order_transaction.php")
                .AddParameter("transaction_id", transaction_id)
                .AddParameter("transaction_date", transaction_date)
                .AddParameter("transaction_payment_type", (int)transaction_payment_type)
                .AddParameter("transaction_order", transaction_order)
                .AddParameter("transaction_cassa", transaction_cassa)
                .AddParameter("transaction_card", transaction_card)
                .AddParameter("transaction_cash_amount", transaction_cash_amount)
                .AddParameter("transaction_card_amount", transaction_card_amount);

            var res = await client.PostAsync(req);
            return res.IsSuccessful;
        }

        // Удаление транзакции
        public async Task<bool> OnDeleteTransactionAsync()
        {
            var req = new RestRequest("/delete_transaction.php")
                .AddParameter("transaction_id", transaction_id);

            var res = await client.PostAsync(req);
            return res.IsSuccessful;
        }

        // Получение транзакций по ID заказа
        public async Task<List<OrderTransaction>> OnSelectTransactionByOrderAsync(int order_id)
        {
            var req = new RestRequest("/select_transaction_by_order.php")
                .AddParameter("transaction_order", order_id);

            var res = await client.PostAsync(req);
            return DataSQL.Deserialize<OrderTransaction>(res.Content);
        }

        // Пакетное получение транзакций для списка заказов
        public async Task<List<OrderTransaction>> OnSelectTransactionBatchAsync(List<int> orderIds)
        {
            var req = new RestRequest("/select_transaction_batch.php")
                .AddParameter("transaction_orders", string.Join(",", orderIds));

            var res = await client.PostAsync(req);
            return DataSQL.Deserialize<OrderTransaction>(res.Content);
        }

    }
}