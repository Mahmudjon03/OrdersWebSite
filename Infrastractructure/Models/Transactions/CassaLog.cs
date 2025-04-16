using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantLibrary.Database;
using RestaurantLibrary.Helpers.Enums;
using RestSharp;

namespace RestaurantLibrary.Models.Transactions
{
    public class CassaLog : TransactionLog
    {
        public int transaction_cassa { get; set; }
        public int transaction_card { get; set; }
        public string transaction_cassa_description { get; set; }
        public EnumCassaOperationType transaction_cassa_operation { get; set; }

        public Cassa cassa { get; set; }
        public Card card { get; set; }

        public string GetCassaDescription { get { return string.IsNullOrEmpty(transaction_cassa_description) ? "-" : transaction_cassa_description; } }
        public string GetSourceString
        {
            get
            {
                if (cassa != null)
                    return cassa.cassa_name;

                if (card != null)
                    return card.card_name;

                return "Неизвестный источник";
            }
        }

        public string TransactionCassaOperationString =>
            Enum.IsDefined(typeof(EnumCassaOperationType), transaction_cassa_operation)
                ? transaction_cassa_operation.ToString()
                : "Неизвестная операция";

        public new async Task<List<CassaLog>> OnLoadTransactionsAsync()
        {
            var req = new RestRequest("/load_cassa_transactions.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<CassaLog>(res.Content);

            return source;
        }
    }
}