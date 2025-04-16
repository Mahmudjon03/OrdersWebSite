using Newtonsoft.Json;
using RestSharp;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System;
using RestaurantLibrary.Helpers.Enums;
using RestaurantLibrary.Database;

namespace RestaurantLibrary.Models
{
    public class User
    {

        [JsonProperty(PropertyName = "user_name")]
        public string user_name { get; set; }

        [JsonProperty(PropertyName = "user_id")]
        public int user_id { get; set; }

        [JsonProperty(PropertyName = "user_password")]
        public string user_password { get; set; }

        [JsonProperty(PropertyName = "user_role")]
        public int user_role { get; set; }
        public int user_kitchen { get; set; }

        public double user_salary { get; set; }
        public int user_salary_type { get; set; }
        public string user_last_payment { get; set; }
        public double user_earnings { get; set; }
        public double user_paid { get; set; }
        public double user_bonus { get; set; }
        public double user_fine { get; set; }

        public List<Shift> shift { get; set; }
        public Role role { get; set; }
        public List<Order> order { get; set; }

        public double UserSumma { get { return user_earnings + user_bonus - user_fine; } }
        public double UserBalance { get { return UserSumma - user_paid; } }
        public string UserRoleName { get { return role?.role_name; } }

        public string OnUserSalary
        {
            get
            {
                string type = "";
                switch (user_salary_type)
                {
                    case (int)EnumSalaryType.Daily:
                        type = $"{user_salary}/день";
                        break;
                    case (int)EnumSalaryType.Monthly:
                        type = $"{user_salary}/месяц";
                        break;
                    case (int)EnumSalaryType.Percent:
                        type = $"{user_salary}%";
                        break;
                }
                return type;
            }
        }

        private static RestClient client = new RestClient(DataSQL.URL + @"/user");


        public async Task<User> OnSelectUserAsync(int id)
        {
            var req = new RestRequest("/select_user.php")
                .AddParameter("user_id", id);

            var res = await client.PostAsync(req); // Используем асинхронный метод

            var source = DataSQL.Deserialize<User>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<User> OnSelectPasswordAsync(string password)
        {
            var req = new RestRequest("/select_password_user.php")
                .AddParameter("user_password", password);

            var res = await client.PostAsync(req); // Используем асинхронный метод

            var source = DataSQL.Deserialize<User>(res.Content);

            return source.Count > 0 ? source[0] : null;
        }

        public async Task<bool> ClearAllDataAsync()
        {
            var req = new RestRequest("/clear_all_data_db.php");

            var res = await client.GetAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnDeleteUserAsync(int id)
        {
            var req = new RestRequest("/delete_user.php")
                .AddParameter("user_id", id);

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> OnUpdateAsync(User user)
        {
            var req = new RestRequest("/update_user.php")
                .AddParameter("user_name", user.user_name)
                .AddParameter("user_password", user.user_password)
                .AddParameter("user_role", user.user_role)
                .AddParameter("user_earnings", user.user_earnings)
                .AddParameter("user_paid", user.user_paid)
                .AddParameter("user_bonus", user.user_bonus)
                .AddParameter("user_fine", user.user_fine)
                .AddParameter("user_salary", user.user_salary)
                .AddParameter("user_salary_type", user.user_salary_type)
                .AddParameter("user_last_payment", user.user_last_payment)
                .AddParameter("user_kitchen", user.user_kitchen)
                .AddParameter("user_id", user.user_id);

            var res = await client.PostAsync(req); // Используем асинхронный метод

            return res.IsSuccessful; // Возвращаем результат напрямую
        }

        public async Task<bool> OnInsertAsync(User user)
        {
            var req = new RestRequest("/insert_user.php")
                .AddParameter("user_name", user.user_name)
                .AddParameter("user_password", user.user_password)
                .AddParameter("user_salary", user.user_salary)
                .AddParameter("user_salary_type", user.user_salary_type)
                .AddParameter("user_last_payment", user.user_last_payment)
                .AddParameter("user_role", user.user_role)
                .AddParameter("user_kitchen", user.user_kitchen);

            var res = await client.PostAsync(req); // Используем асинхронный метод

            return res.IsSuccessful; // Возвращаем результат напрямую
        }

        public async Task<List<User>> OnAllUserAsync(bool superAdmin = false)
        {
            var req = superAdmin
                ? new RestRequest("/load_all.php")
                : new RestRequest("/all_workers.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<User>(res.Content);

            return source;
        }

        public async Task<List<User>> OnLoadSalaryAsync()
        {
            var req = new RestRequest("/select_users_salary.php");

            var res = await client.GetAsync(req);

            var source = DataSQL.Deserialize<User>(res.Content);

            return source;
        }

        public async Task<bool> OnMigrate()
        {
            var req = new RestRequest("/migrate.php");

            var res = await client.PostAsync(req);

            return res.IsSuccessful;
        }

        public async Task<bool> ExportDump(string path)
        {
            // Убедитесь, что путь корректен
            path = path.Replace("\\", "/"); // Заменяем обратные слэши на обычные

            var req = new RestRequest("/import_dump.php")
                .AddParameter("backup_file", path);

            var res = await client.PostAsync(req);

            if (res.IsSuccessful)
            {
                Console.WriteLine(res.Content); // Логируем ответ
                return res.Content.Contains("success");
            }

            return false;
        }

        public override string ToString()
        {
            return user_name;
        }
    }
}
