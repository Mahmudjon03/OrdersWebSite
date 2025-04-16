namespace RestaurantLibrary.Helpers
{
    using System.IO;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class GridColumnSettings
    {
        private readonly string _filePath;

        public GridColumnSettings(string filePath)
        {
            _filePath = filePath;
        }

        public void SaveSettings(Dictionary<string, bool> columnSettings)
        {
            var json = JsonConvert.SerializeObject(columnSettings);
            File.WriteAllText(_filePath, json);
        }

        public Dictionary<string, bool> LoadSettings()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
            }
            return new Dictionary<string, bool>();
        }
    }
}
