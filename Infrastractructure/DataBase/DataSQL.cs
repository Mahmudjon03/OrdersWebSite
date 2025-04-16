using Newtonsoft.Json;
using RestaurantLibrary.Helpers;
using System.Text;

namespace RestaurantLibrary.Database
{
    public static class DataSQL
    {
        public static string URL = string.Format("http://192.168.128.57/android/php");
        //public static string URL = string.Format("https://cg97830.tw1.ru/android/php/");

        public static string ConvertDouble(double text)
        {
            return text.ToString().Replace(",", ".");
        }

        public static SettingsJson OnMyConfig()
        {
            var path = Environment.CurrentDirectory + @"\Config.json";
            var json = File.ReadAllText(path);
            SettingsJson config = JsonConvert.DeserializeObject<SettingsJson>(json);

            return config;
        }

        public static List<T> Deserialize<T>(this string SerializedJSONString)
        {
            var result = SerializedJSONString.Replace(@"\\u", @"\u");

            try
            {
                result = UnicodeToUTF8(result);

                result = result.Replace("\\\"", "");
                result = result.Remove(0, 1);
                result = result.Substring(0, result.Length - 1);

                var stuff = JsonConvert.DeserializeObject<List<T>>(result);
                return stuff;
            }
            catch(Exception ex)
            {
                UserNotifier.Notifier?.ShowError($"Oшибка при десериализации (тип: {typeof(T).Name}): {ex.Message}");
                return null;
            }
        }

        static string UnicodeToUTF8(string from)
        {
            var bytes = Encoding.UTF8.GetBytes(from);
            return new string(bytes.Select(b => (char)b).ToArray());
        }
    }
}
