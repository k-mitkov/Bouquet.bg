using System;
using System.Collections.Generic;
using System.Text;

namespace Bouquet.Mobile.Models
{
    public class CartBouquetDTO
    {
        public BouquetDTO Bouquet { get; set; }

        public int Count { get; set; }
    }
}
