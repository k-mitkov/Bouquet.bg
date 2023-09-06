using System;
using System.Collections.Generic;

namespace Bouquet.Mobile.Models
{
    public class OrderDTO
    {
        public string Id { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string UserID { get; set; }

        public int Status { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public string Name { get; set; }

        public string ReciverPhoneNumber { get; set; }

        public string ReciverName { get; set; }

        public DateTime PreferredTime { get; set; }

        public int Type { get; set; }

        public IEnumerable<CartBouquetDTO> Bouquets { get; set; }
    }
}
