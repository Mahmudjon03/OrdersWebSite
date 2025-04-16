using RestaurantLibrary.Helpers.Enums;
using System.Collections.Generic;

namespace RestaurantLibrary.Models
{
    public class HallType
    {
        public int type_id { get; set; }
        public string type_name { get; set; }

        public List<HallType> OnLoad()
        {
            return new List<HallType>()
            {
                new HallType() { type_id = (int) EnumHallType.Free, type_name = "Без оплаты",},
                new HallType() { type_id = (int) EnumHallType.Fixed, type_name = "Фикс плата" },
                new HallType() { type_id = (int) EnumHallType.TimeBased, type_name = "Оплата за время" },
            };
        }
    }
}
