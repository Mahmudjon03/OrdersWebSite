using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using RestaurantLibrary.Helpers;
using RestaurantLibrary.Models;

namespace OrdersApp.Controllers
{
    public class OrdersController : Controller
    {
        public List<Food> foods;

        public OrdersController()
        {
            {
                foods = new List<Food>
                    {
                      new Food
                    {
                        Id = 1,
                        Name = "Coca-Cola",
                        Description = "Классический газированный напиток с освежающим вкусом. Идеален к любому блюду.",
                        ImagePath = "~/image/ccole.jpg",
                        Price = 9.99m,
                        Type = 3
                    },
                    new Food
                    {
                        Id = 2,
                        Name = "Чай",
                        Description = "Горячий чёрный чай, придающий бодрость и тепло. Отличный выбор для любого времени дня.",
                        ImagePath = "~/image/choy.jpg",
                        Price = 4.50m,
                        Type = 3
                    },
                    new Food
                    {
                        Id = 3,
                        Name = "Fanta",
                        Description = "Яркий апельсиновый вкус и пузырьки делают Fanta любимым напитком детей и взрослых.",
                        ImagePath = "~/image/fanta.jpg",
                        Price = 8.99m,
                        Type = 3
                    },
                    new Food
                    {
                        Id = 4,
                        Name = "RS",
                        Description = "Свежевыжатый сок, полный витаминов и натуральной энергии. Без сахара и добавок.",
                        ImagePath = "~/image/rs.jpg",
                        Price = 10.50m,
                        Type = 3
                    },
                    new Food
                    {
                        Id = 5,
                        Name = "Лимонный чай",
                        Description = "Ароматный чай с долькой лимона — освежает, тонизирует и согревает.",
                        ImagePath = "~/image/limonchoy.jpg",
                        Price = 5.00m,
                        Type = 3
                    },

                    new Food
                    {
                        Id = 6,
                        Name = "Салат 1",
                        Description = "Лёгкий салат из свежих помидоров, огурцов и зелени. Отличный гарнир.",
                        ImagePath = "~/image/salat1.jpg",
                        Price = 12.99m,
                        Type = 2
                    },
                    new Food
                    {
                        Id = 7,
                        Name = "Салат 2",
                        Description = "Овощной микс с оливковым маслом и приправами — вкусно и полезно.",
                        ImagePath = "~/image/salat2.jpg",
                        Price = 11.50m,
                        Type = 2
                    },
                    new Food
                    {
                        Id = 8,
                        Name = "Салат 3",
                        Description = "Сытный салат с добавлением сыра, зелени и нежных овощей.",
                        ImagePath = "~/image/salat3.jpg",
                        Price = 13.75m,
                        Type = 2
                    },
                    new Food
                    {
                        Id = 9,
                        Name = "Салат 4",
                        Description = "Капустный салат с морковью и лёгкой заправкой — хрустящий и свежий.",
                        ImagePath = "~/image/salat4.jpg",
                        Price = 10.00m,
                        Type = 2
                    },

                     new Food
                    {
                        Id = 10,
                        Name = "Шурма",
                        Description = "Ароматная шурма с мясом, овощами и соусом, завернутая в тёплый лаваш.",
                        ImagePath = "~/image/shurma2.jpg",
                        Price = 20.99m,
                        Type = 1
                        },
                    new Food
                    {
                        Id = 11,
                        Name = "Тантуни",
                        Description = "Тантуни — это турецкое блюдо из тонко нарезанного мяса, обжаренного на сковороде и завернутого в лаваш. Подаётся с овощами и специями. Очень вкусное и сытное!",
                        ImagePath = "~/image/tantuni.jpg",
                        Price = 22.50m,
                        Type = 1
                        },
                    new Food
                    {
                        Id = 12,
                        Name = "Тантуни 2",
                        Description = "Нежное куриное филе с овощами в тонком лаваше. Лёгкий и сытный обед.",
                        ImagePath = "~/image/tantuni2.jpg",
                        Price = 21.00m,
                        Type = 1
                    },
                      new Food
                    {
                        Id = 12,
                        Name = "Тантуни 3",
                        Description = "Нежное куриное филе с овощами в тонком лаваше. Лёгкий и сытный обед.",
                        ImagePath = "~/image/tantuni3.jpg",
                        Price = 21.00m,
                        Type = 1
                    },
                    new Food
                    {
                        Id = 13,
                        Name = "Хот-дог",
                        Description = "Классический хот-дог с сочной сосиской, хрустящей булочкой и горчичным соусом.",
                        ImagePath = "~/image/xodog.jpg",
                        Price = 18.75m,
                        Type = 1
                        }
                    };
            };
        }
        public async Task<IActionResult> OrderFood(int table = 1)
        {
            HttpContext.Session.SetInt32("CurrentTable", table);
            var reversedFoods = foods.OrderByDescending(f => f.Id).ToList();

            var product = await new Product().OnLoadAsync();
            foreach(var prod in product)
            {
                prod.prod_name = TransliterationHelper.CyrillicToLatin(prod.prod_name);
            }
            return View(product);
        }
        public IActionResult OrderDrink()
        {
            var reversedFoods = foods.OrderBy(f => f.Id).ToList();

            return View(reversedFoods);
        }
        public IActionResult OrderSalad()
        {
            return View();
        }

        public IActionResult Basket()
        {
            return View();
        }






    }
}
