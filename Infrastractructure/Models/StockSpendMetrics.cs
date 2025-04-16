namespace RestaurantLibrary.Models
{
    public class StockSpendMetrics
    {
        public string prod_name { get; set; }
        public double prod_total { get; set; }
        public double stock_total { get; set; }
        public double stock_price { get; set; }
        public string order_num { get; set; }
        public string date { get; set; }
    }
}
