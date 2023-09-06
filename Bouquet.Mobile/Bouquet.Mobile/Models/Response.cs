using Bouquet.Mobile.Enums;

namespace Bouquet.Mobile.Models
{
    /// <summary>
    /// Base response for all types of requests
    /// </summary>
    public class Response
    {
        /// <summary>
        /// Response status
        /// </summary>
        public StatusEnum Status { get; set; }

        /// <summary>
        /// Optional message
        /// </summary>
        public string Message { get; set; }
    }

    public class Response<T> : Response
    {
        public T Data { get; set; } = default;
    }
}
