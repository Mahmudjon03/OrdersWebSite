using Microsoft.AspNetCore.Mvc;

namespace OrdersApp.Controllers
{
    public class OrdersController: Controller
    {
        public IActionResult OrderFood()
        {
            return View();
        }
        public IActionResult OrderDrink()
        {
            return View();
        }
        public IActionResult OrderSalad()
        {
            return View();
        }






    }
}
