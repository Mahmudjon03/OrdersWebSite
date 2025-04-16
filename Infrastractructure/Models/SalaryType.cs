using RestaurantLibrary.Helpers.Enums;
using System.Collections.Generic;

namespace RestaurantLibrary.Models
{
    public class SalaryType
    {
        public EnumSalaryLogType type { get; set; }
        public string type_name { get; set; }

        public List<SalaryType> OnLoadPay()
        {
            return new List<SalaryType>()
            {
                new SalaryType() { type = EnumSalaryLogType.Оплата, type_name = "Оплата",},
                new SalaryType() { type = EnumSalaryLogType.Предоплата, type_name = "Предоплата" },
            };
        }
        public List<SalaryType> OnLoadEarnings()
        {
            return new List<SalaryType>()
            {
                new SalaryType() { type = EnumSalaryLogType.Бонус, type_name = "Бонус",},
                new SalaryType() { type = EnumSalaryLogType.Штраф, type_name = "Штраф",},
            };
        }
    }
}
