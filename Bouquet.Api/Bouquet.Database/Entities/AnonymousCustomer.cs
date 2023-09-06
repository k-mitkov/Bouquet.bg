namespace Bouquet.Database.Entities
{
    public class AnonymousCustomer
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string SecondName { get; set; }

        public string PhoneNumber { get; set; }

        #region Navigation database properties

        /// <summary>
        /// Поръчки
        /// </summary>
        public IEnumerable<Order>? Orders { get; set; }

        #endregion

        public AnonymousCustomer()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
