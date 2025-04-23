using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using RestaurantLibrary.Helpers;
using RestaurantLibrary.Models;

namespace OrdersApp.Controllers
{
    public class OrdersController : Controller
    {
        public async Task<IActionResult> OrderFood(int? table, int category = 33)
        {
            if (table.HasValue)
            {
                HttpContext.Session.SetInt32("CurrentTable", table.Value);
            }
            else
            {
                table = HttpContext.Session.GetInt32("CurrentTable") ?? 1;
            }

            var product = await new Product().OnLoadAsync();
            var result = product.Where(x => x.prod_category == category).ToList();

            return View(result);
        }
    }
}
