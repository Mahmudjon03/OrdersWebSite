using RestaurantLibrary.Helpers;

namespace Domain.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string EnglishNameProd { get {
             return TransliterationHelper.CyrillicToLatin(ProductName);
            }
        }

        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}
