using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using RestaurantLibrary.Helpers;
using RestaurantLibrary.Helpers.Enums;
using RestaurantLibrary.Models;


namespace OrdersApp.Controllers
{
    public class BasketController : Controller
    {
        public IActionResult Index()
        {
            var basket = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Basket") ?? new List<CartItem>();
            return View(basket);
        }

        [HttpGet]
        public IActionResult GetCartCount()
        {
            var basket = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Basket") ?? new List<CartItem>();
            int count = basket.Count;
            return Json(count);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderDetails(List<CartItem> model)
        {
            if (model == null || model.Count == 0)
            {
                return BadRequest("Basket is empty");
            }
            int orderId = await GetCurrentOrderId();
            var orders = new Order()
            {
                order_main = orderId,
                order_user = -1,
                order_shift = 1,
                order_date = DateTimeHelper.GetCurrentDateTimeString(),
                order_close_date = DateTimeHelper.GetCurrentDateTimeString(),
                order_price = 100,
                order_table = HttpContext.Session.GetInt32("CurrentTable") ?? 0,
                order_delivery = (int)EnumOrderType.Default,
                order_payment = 1,
                order_status = 1,
                order_status_cook = (int)EnumDetailsStatus.Ready,
                order_comment = "mesage",
                order_discount = 1.2
            };
            var result = await new Order().OnInsertAsync(orders);

            OrderDetails detail;
            var dbDetails = new OrderDetails();
            foreach (var i in model)
            {
                detail = new OrderDetails()
                {
                    details_order = orderId,
                    details_prod = i.ProductId,
                    details_count = i.Quantity,
                    details_comment = "comment",
                    details_status = (int)EnumDetailsStatus.New,
                };
                var result2 = await dbDetails.OnInsertAsync(detail);
            }

            HttpContext.Session.Remove("Basket");

            return RedirectToAction("OrderFood", "Orders");
        }


        private async Task<int> GetCurrentOrderId()
        {
            return await new Que().GetQueNumAsync();
        }


        [HttpPost]
        public IActionResult AddToBasket(int id, string name, decimal price)
        {
            var basket = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Basket") ?? new List<CartItem>();

            var existingItem = basket.FirstOrDefault(x => x.ProductId == id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                basket.Add(new CartItem
                {
                    ProductId = id,
                    ProductName = name,
                    Price = price,
                    Quantity = 1
                });
            }

            HttpContext.Session.SetObjectAsJson("Basket", basket);
            return Ok();
        }

        [HttpPost]

        public IActionResult IncreaseQuantity([FromBody] RemoveRequest prod)
        {
            var basket = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Basket") ?? new List<CartItem>();

            var item = basket.FirstOrDefault(x => x.ProductId == prod.ProductId);
            if (item != null)
            {
                item.Quantity++;
                HttpContext.Session.SetObjectAsJson("Basket", basket);
                return Json(new { success = true, quantity = item.Quantity });
            }

            return Json(new { success = false });
        }
        [HttpPost]
        public IActionResult DecreaseQuantity([FromBody] RemoveRequest prod)
        {
            var basket = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Basket") ?? new List<CartItem>();

            var item = basket.FirstOrDefault(x => x.ProductId == prod.ProductId);
            if (item != null && item.Quantity > 1)
            {
                item.Quantity--;
                HttpContext.Session.SetObjectAsJson("Basket", basket);
                return Json(new { success = true, quantity = item.Quantity });
            }

            return Json(new { success = false });
        }


        [HttpPost]
        public IActionResult RemoveFromCart([FromBody] RemoveRequest product)
        {
            var basket = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Basket") ?? new List<CartItem>();

            var itemToRemove = basket.FirstOrDefault(item => item.ProductId == product.ProductId);
            if (itemToRemove != null)
            {
                basket.Remove(itemToRemove);
                HttpContext.Session.SetObjectAsJson("Basket", basket);
            }

            return Json(new { success = true });
        }

        [HttpPost]

        public IActionResult SetQuantity([FromBody] SetQuantityRequest request)
        {

            if (request.Quantity < 1) request.Quantity = 1;

            var baskets = HttpContext.Session.GetObjectFromJson<List<CartItem>>("Basket") ?? new List<CartItem>();

            var basket = baskets.FirstOrDefault(item => item.ProductId == request.ProductId);
            if (basket != null)
            {
                basket.Quantity = request.Quantity;
                HttpContext.Session.SetObjectAsJson("Basket", baskets);
            }
            return Json(new { success = true, quantity = request.Quantity });
        }

        public class SetQuantityRequest
        {
            public int ProductId { get; set; }
            public int Quantity { get; set; }
        }
        public class RemoveRequest
        {
            public int ProductId { get; set; }
        }

    }
}
