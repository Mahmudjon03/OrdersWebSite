using RestaurantLibrary.Database;
using RestSharp;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestaurantLibrary.Models
{
    public class Tables
    {
        public int table_id { get; set; }
        public string table_name { get; set; }
        public int table_place { get; set; }
        public int table_hall_id { get; set; }
        public int table_status { get; set; }
        public string table_date { get; set; }
        public string table_time { get; set; }

        public TableStatus tables_status { get; set; }
        public Hall hall { get; set; }
        public string GetHallName { get { return hall?.hall_name; } }
        public string GetTableStatusName { get { return tables_status?.table_st_name; } }

        private static RestClient client = new RestClient(DataSQL.URL + @"/table");


        public async Task<List<Tables>> OnLoadAsync()
        {
            var req = new RestRequest("/load_tables.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Tables>(res.Content);

            return source;
        }

        public async Task<Tables> OnSelectAsync()
        {
            var req = new RestRequest("/select_tables.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<Tables>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<Tables> ChecKTableAsync(string tableName, int hallId)
        {
            var req = new RestRequest("/check_table.php")
                .AddParameter("table_name", tableName)
                .AddParameter("table_hall_id", hallId);

            var res = await client.PostAsync(req);

            var source = DataSQL.Deserialize<Tables>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> OnDeleteAsync(Tables tables, int userId)
        {
            var req = new RestRequest("/delete_table.php")
                .AddParameter("table_id", tables.table_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                await Debug.DebugInsertAsync($"Удалил столик: {tables.table_name}\nid зала: {tables.table_hall_id}", userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdateAsync(Tables tables, int userId)
        {
            var req = new RestRequest("/update_tables.php")
                .AddParameter("table_hall_id", tables.table_hall_id)
                .AddParameter("table_name", tables.table_name)
                .AddParameter("table_place", tables.table_place)
                .AddParameter("table_id", tables.table_id);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                var str = string.Format("Изменил данные столика: {0}\nКол-во мест: {1}\nНаименование: {2}\nid зала: {3}",
                    tables.table_name, tables.table_place, tables.table_name, tables.table_hall_id);
                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
            return false;
        }

        public async Task<bool> OnUpdateStatusAsync(Tables tables, int userId)
        {
            RestRequest req = new RestRequest("/update_tables_status.php")
                .AddParameter("table_status", tables.table_status)
                .AddParameter("table_date", tables.table_date)
                .AddParameter("table_time", tables.table_time)
                .AddParameter("table_id", tables.table_id);

            var res = await client.PostAsync(req);

            if (!res.IsSuccessful)
            {
                return false;
            }
            else
            {
                var str = string.Format("Изменил статус столика id: {0}\nСтатус id: {1}\nДата: {2}\nВремя: {3}",
                    tables.table_id, tables.table_status, tables.table_date, tables.table_time);

                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
        }

        public async Task<bool> OnInsertAsync(Tables tables, int userId)
        {
            var req = new RestRequest("/insert_tables.php")
                .AddParameter("table_id", tables.table_id)
                .AddParameter("table_hall_id", tables.table_hall_id)
                .AddParameter("table_name", tables.table_name)
                .AddParameter("table_date", tables.table_date)
                .AddParameter("table_time", tables.table_time)
                .AddParameter("table_place", tables.table_place);

            var res = await client.PostAsync(req);

            if (!res.IsSuccessful)
            {
                return false;
            }
            else
            {
                var str = string.Format("Добавил новый столик\n" +
                    "Кол-во мест: {0}\n" +
                    "Наименование: {1}\n" +
                    "Зал: {2}",
                    tables.table_place, tables.table_name, tables.table_hall_id);

                await Debug.DebugInsertAsync(str, userId);
                return true;
            }
        }
    }
}
