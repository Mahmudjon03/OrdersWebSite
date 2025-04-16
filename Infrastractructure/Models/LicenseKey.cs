namespace RestaurantLibrary.Models
{
    public class LicenseKey
    {
        public string keyExpire { get; set; }
        public string keyValue { get; set; }
        public bool isActive { get; set; }
    }
}