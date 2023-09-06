using AutoMapper;
using Bouquet.Database;
using Bouquet.Database.Entities;
using Bouquet.Services.Interfaces.Order;
using Bouquet.Services.Models.DTOs;
using Bouquet.Services.Models.Requests;
using Bouquet.Services.Models.Responses;
using Bouquet.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Bouquet.Services.Order
{
    public class OrderService : IOrderService
    {
        #region Declarations

        private readonly BouquetContext _dbContext;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor

        public OrderService(BouquetContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Взима поръчки
        /// </summary>
        /// <returns></returns>
        public async Task<Response> GetOrdersAsync(string shopID, string userId)
        {
            if (!string.IsNullOrEmpty(shopID))
            {
                return new Response<IEnumerable<OrderDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<IEnumerable<OrderDTO>>(await _dbContext.Orders.Include(o => o.OrderCart).ThenInclude(c => c.Bouquets).ThenInclude(b => b.Bouquet).ThenInclude(b => b.Pictures).Include(o => o.User).ThenInclude(u => u.UserInfo).Include(o => o.DeliveryDetails).Where(o => o.FlowerShopID == shopID).ToListAsync()) };
            }
            else if (!string.IsNullOrEmpty(userId))
            {
                return new Response<IEnumerable<OrderDTO>> { Status = StatusEnum.Success, Data = _mapper.Map<IEnumerable<OrderDTO>>(await _dbContext.Orders.Include(o => o.OrderCart).ThenInclude(c => c.Bouquets).ThenInclude(b => b.Bouquet).ThenInclude(b => b.Pictures).Include(o => o.User).ThenInclude(u => u.UserInfo).Include(o => o.DeliveryDetails).Where(o => o.UserID == userId).ToListAsync()) };
            }
            else
            {
                return new Response { Status = StatusEnum.Failure, Message = "Invalid input" };
            }
        }

        /// <summary>
        /// Променя статуса
        /// </summary>
        /// <returns></returns>
        public async Task<Response> UpdateStatus(string orderID, int? status)
        {
            if (string.IsNullOrEmpty(orderID) || !status.HasValue)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Invalid input" };
            }

            var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderID);

            if (order == null)
            {
                return new Response { Status = StatusEnum.Failure, Message = "Invalid input" };
            }

            order.Status = status.Value;

            _dbContext.Update(order);
            await _dbContext.SaveChangesAsync();

            return new Response { Status = StatusEnum.Success };
        }

        public async Task<Response> MakeAnonymousOrder(MakeOrderRequest orderRequest)
        {
            throw new NotImplementedException();
        }

        public async Task<Response> MakeOrder(MakeOrderRequest orderRequest, string email)
        {
            try
            {
                if (orderRequest == null || orderRequest.Bouquets == null || !orderRequest.Bouquets.Any())
                {
                    return new Response { Status = StatusEnum.Failure, Message = "Invalid input" };
                }

                var user = await _dbContext.Users.Include(u => u.UserInfo).FirstOrDefaultAsync(u => u.Email!.ToLower() == email.ToLower());

                var order = new Database.Entities.Order();

                order.UserID = orderRequest.UserId;
                order.FlowerShopID = orderRequest.ShopId;
                order.Description = orderRequest.Description;
                order.Price = orderRequest.Price;

                var cart = new OrderCart();
                var bouquetdInCart = new List<CartBouquet>();

                foreach (var bouquet in orderRequest.Bouquets)
                {
                    var bouquetFromDB = await _dbContext.Bouquets.FirstOrDefaultAsync(b => b.Id == bouquet.Bouquet.Id);

                    if (bouquetFromDB != null)
                    {
                        var cartBouquet = new CartBouquet();
                        cartBouquet.BouquetID = bouquetFromDB.Id;
                        cartBouquet.Bouquet = bouquetFromDB;
                        cartBouquet.Count = bouquet.Count;
                        bouquetdInCart.Add(cartBouquet);
                    }
                }

                var deliveryDetails = new DeliveryDetails();

                if (orderRequest.HasDelivery)
                {
                    order.Status = (int)OrderStatusEnum.New;

                    deliveryDetails.Address = orderRequest.Address;
                    deliveryDetails.Type = (int)DeliveryDetailsType.ToAddress;
                }
                else
                {
                    order.Status = (int)OrderStatusEnum.NotPaid;

                    deliveryDetails.Address = "";
                    deliveryDetails.Type = (int)DeliveryDetailsType.FromStore;
                }

                deliveryDetails.PhoneNumber = orderRequest.ReciverPhoneNumber ?? user.PhoneNumber ?? "";
                deliveryDetails.ReciverName = orderRequest.ReciverName ?? user.UserInfo.FirstName ?? "";
                deliveryDetails.UserID = orderRequest.UserId;


                _dbContext.DeliveryDetails.Add(deliveryDetails);

                order.DeliveryDetailsID = deliveryDetails.Id;

                cart.Bouquets = bouquetdInCart;
                order.OrderCart = cart;
                cart.Order = order;

                _dbContext.Orders.Add(order);

                order.OrderCart.OrderID = order.Id;

                _dbContext.OrderCarts.Add(cart);
                _dbContext.SaveChanges();

                return new Response<string> { Status = StatusEnum.Success, Data = order.Id };
            }
            catch (Exception ex)
            {
                return new Response { Status = StatusEnum.Failure, Message = ex.Message };
            }
        }

        #endregion
    }
}
