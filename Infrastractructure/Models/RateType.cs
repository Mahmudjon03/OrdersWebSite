using RestaurantLibrary.Helpers.Enums;
using System.Collections.Generic;

namespace RestaurantLibrary.Models
{
    public class RateType
    {
        public int rate_id { get; set; }
        public string rate_name { get; set; }
        public int rate_interval_hour { get; set; }

        public List<RateType> OnLoad()
        {
            return new List<RateType>()
            {
                new RateType() { rate_id = (int) EnumRateType.Hour, rate_name = "Каждый час.", rate_interval_hour = 1, },
                new RateType() { rate_id = (int) EnumRateType.Hour3, rate_name = "Раз в 3 часа.", rate_interval_hour = 3, },
                new RateType() { rate_id = (int) EnumRateType.Hour6, rate_name = "Раз в 6 часов.", rate_interval_hour = 6, },
                new RateType() { rate_id = (int) EnumRateType.Hour12, rate_name = "Раз в 12 часов.", rate_interval_hour = 12, },
                new RateType() { rate_id = (int) EnumRateType.Day, rate_name = "Ежедневно.", rate_interval_hour = 24,  },
            };
        }
    }
}
