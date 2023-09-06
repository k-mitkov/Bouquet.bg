using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Bouquet.Mobile.Models
{
    public class BouquetDTO
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int FlowersCount { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string FlowerShopID { get; set; }

        /// <summary>
        /// Магазин
        /// </summary>
        public FlowerShopDTO FlowerShop { get; set; }

        /// <summary>
        /// Снимки
        /// </summary>
        public IEnumerable<PictureDTO> Pictures { get; set; }

        public ImageSource Picture {
            get
            {
                var url = Pictures.FirstOrDefault() != null ? AppConstands.Url + "/" + Pictures.FirstOrDefault().PictureDataUrl : "https://cdn-icons-png.flaticon.com/512/3050/3050959.png";
                return ImageSource.FromUri(new Uri(url));
            }
        } 

        /// <summary>
        /// Цветя
        /// </summary>
        public IEnumerable<FlowerDTO> Flowers { get; set; }

        /// <summary>
        /// Цветове
        /// </summary>
        public IEnumerable<ColorDTO> Colors { get; set; }
    }
}
