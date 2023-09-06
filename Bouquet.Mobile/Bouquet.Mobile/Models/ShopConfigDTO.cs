using System;

namespace Bouquet.Mobile.Models
{
    public class ShopConfigDTO
    {
        public string Id { get; set; }

        public TimeSpan OpenAt { get; set; }

        public TimeSpan CloseAt { get; set; }

        public TimeSpan SameDayTillHour { get; set; }

        public double Price { get; set; }

        public double FreeDeliveryAt { get; set; }
    }
}
