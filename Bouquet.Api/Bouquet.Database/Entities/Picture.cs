using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bouquet.Database.Entities
{
    public class Picture
    {
        public string Id { get; set; }

        [Column(TypeName = "text")]
        public string PictureDataUrl { get; set; }

        public string BouquetID { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Букет
        /// </summary>
        public Bouquet? Bouquet { get; set; }

        #endregion

        public Picture()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
