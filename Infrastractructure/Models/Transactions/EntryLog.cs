using System.Collections.Generic;
using System.Threading.Tasks;
using RestSharp;
using RestaurantLibrary.Database;

namespace RestaurantLibrary.Models.Transactions
{
    public class EntryLog : TransactionLog
    {
        public int transaction_entry { get; set; } // Для связи с приходом
        public string transaction_entry_description { get; set; }

        public async Task<List<EntryLog>> OnSelectEntryTransactionsAsync(int entryId)
        {
            var req = new RestRequest("/select_entry_transaction.php")
                .AddParameter("transaction_entry", entryId);

            var res = await client.PostAsync(req);

            return DataSQL.Deserialize<EntryLog>(res.Content);
        }
    }
}
