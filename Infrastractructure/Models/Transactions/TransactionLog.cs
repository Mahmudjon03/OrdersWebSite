using RestSharp;
using System.Threading.Tasks;
using System.Collections.Generic;
using RestaurantLibrary.Helpers.Enums;
using RestaurantLibrary.Database;

namespace RestaurantLibrary.Models.Transactions
{
    public class TransactionLog
    {
        public int transaction_id { get; set; }
        public int transaction_user { get; set; }
        public EnumTransactionType transaction_type { get; set; }
        public double transaction_amount { get; set; }
        public double transaction_balance { get; set; }
        public string transaction_date { get; set; }
        public string transaction_description { get; set; }

        public User user { get; set; }

        public string GetDescription { get { return string.IsNullOrEmpty(transaction_description) ? "-" : transaction_description; } }

        protected static RestClient client = new RestClient(DataSQL.URL + @"/transactions");

        public async Task<List<TransactionLog>> OnLoadTransactionsAsync()
        {
            var req = new RestRequest("/load_transaction.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<TransactionLog>(res.Content);

            return source;
        }

        public async Task<List<TransactionLog>> OnLoadBalanceTransactionsAsync()
        {
            var req = new RestRequest("/load_balance_transactions.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<TransactionLog>(res.Content);

            return source;
        }

        public async Task<List<TransactionLog>> OnLoadSpendTransactionsAsync()
        {
            var req = new RestRequest("/load_spend_transactions.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<TransactionLog>(res.Content);

            return source;
        }

        public async Task<List<TransactionLog>> OnLoadIncomeTransactionsAsync()
        {
            var req = new RestRequest("/load_income_transactions.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<TransactionLog>(res.Content);

            return source;
        }

        public async Task<TransactionLog> OnSelectTransactionAsync(int transaction_id)
        {
            var req = new RestRequest("/select_transaction.php")
                .AddParameter("transaction_id", transaction_id);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<TransactionLog>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> OnInsertTransactionAsync(TransactionLog transaction)
        {
            var req = new RestRequest("/insert_transaction.php")
                .AddParameter("transaction_user", transaction.transaction_user)
                .AddParameter("transaction_type", (int)transaction.transaction_type)
                .AddParameter("transaction_amount", transaction.transaction_amount)
                .AddParameter("transaction_balance", transaction.transaction_balance)
                .AddParameter("transaction_date", transaction.transaction_date)
                .AddParameter("transaction_description", transaction.transaction_description);

            if (transaction is CassaLog cassaTransaction)
            {
                req.AddParameter("transaction_cassa", cassaTransaction.transaction_cassa)
                   .AddParameter("transaction_card", cassaTransaction.transaction_card)
                   .AddParameter("transaction_cassa_description", cassaTransaction.transaction_cassa_description)
                   .AddParameter("transaction_cassa_operation", (int)cassaTransaction.transaction_cassa_operation);
            }

            if (transaction is EntryLog entryTransaction)
            {
                req.AddParameter("transaction_entry", entryTransaction.transaction_entry)
                    .AddParameter("transaction_entry_description", entryTransaction.transaction_entry_description);
            }

            if (transaction is SalaryLog salaryTransaction)
            {
                req.AddParameter("transaction_salary_type", (int)salaryTransaction.transaction_salary_type)
                    .AddParameter("transaction_salary_user", salaryTransaction.transaction_salary_user)
                    .AddParameter("transaction_salary_description", salaryTransaction.transaction_salary_description);
            }

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnUpdateTransactionAsync(TransactionLog transaction)
        {
            var req = new RestRequest("/update_transaction.php")
                .AddParameter("transaction_id", transaction.transaction_id)
                .AddParameter("transaction_user", transaction.transaction_user)
                .AddParameter("transaction_type", (int)transaction.transaction_type)
                .AddParameter("transaction_amount", transaction.transaction_amount)
                .AddParameter("transaction_balance", transaction.transaction_balance)
                .AddParameter("transaction_date", transaction.transaction_date)
                .AddParameter("transaction_description", transaction.transaction_description);


            if (transaction is CassaLog cassaTransaction)
            {
                req.AddParameter("transaction_cassa", cassaTransaction.transaction_cassa)
                   .AddParameter("transaction_card", cassaTransaction.transaction_card)
                   .AddParameter("transaction_cassa_description", cassaTransaction.transaction_cassa_description)
                   .AddParameter("transaction_cassa_operation", (int)cassaTransaction.transaction_cassa_operation);
            }

            if (transaction is EntryLog entryTransaction)
            {
                req.AddParameter("transaction_entry", entryTransaction.transaction_entry)
                    .AddParameter("transaction_entry_description", entryTransaction.transaction_entry_description);
            }

            if (transaction is SalaryLog salaryTransaction)
            {
                req.AddParameter("transaction_salary_type", (int)salaryTransaction.transaction_salary_type)
                    .AddParameter("transaction_salary_user", salaryTransaction.transaction_salary_user)
                    .AddParameter("transaction_salary_description", salaryTransaction.transaction_salary_description);
            }

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnDeleteTransactionAsync(int transaction_id)
        {
            var req = new RestRequest("/delete_transaction.php")
                .AddParameter("transaction_id", transaction_id);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<TransactionLog> OnSelectLastTransactionAsync()
        {
            var req = new RestRequest("/select_last_transaction.php");

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<TransactionLog>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }
    }
}
