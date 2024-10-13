//using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Interfaces;
using Talabat.BLL.Specifications.Order_Specifications;
using Talabat.DAL.Entities;
using Talabat.DAL.Entities.Order_Aggregate;

namespace Talabat.BLL.Services
{
    //Create Order Using Apstract Factory design pattern
    public class OrderServices : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPaymentService _paymentService;

        //private readonly IGenericRepository<Product> _productsRepo;
        //private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
        //private readonly IGenericRepository<Order> _orderrepo;

        public OrderServices(IBasketRepository basketRepository,
              //IGenericRepository<Product> productsRepo,
              //IGenericRepository<DeliveryMethod> deliveryMethodRepo,
              //IGenericRepository<Order> orderrepo
              IUnitOfWork unitOfWork,
              IPaymentService paymentService
            )
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
            _paymentService = paymentService;
            //_productsRepo = productsRepo;
            //_deliveryMethodRepo = deliveryMethodRepo;
            //_orderrepo = orderrepo;
        }
        public async Task<Order> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
        {
            //1-Get Basket From Basket Repo 
            var basket = await _basketRepository.GetCustomerBasket(basketId);

            //2- Get Selected Items at Basket From Products Repo
            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                var productItemOrder = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);

                var orderItem = new OrderItem(productItemOrder, product.Price, item.Quantity);

                orderItems.Add(orderItem);
            }

            //3- Get Delivery Method From DeliveryMethods Repo 
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            //4- Calculate SubTotal
            var subTotal = orderItems.Sum(item => item.Price * item.Quanity);

            //5-Check If Order is Exist Or Null
            var spec = new OrderWithItemByPaymentIntentSpecifications(basket.PaymentIntentId);
            var existingOrder = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);
            if (existingOrder != null)
            {
                _unitOfWork.Repository<Order>().Delete(existingOrder);

                await _paymentService.CreateOrUpdatePaymentIntent(basketId); 
            }


            //6- Create Order 
            var order = new Order(buyerEmail, shippingAddress, deliveryMethod, orderItems, subTotal, basket.PaymentIntentId);
            await _unitOfWork.Repository<Order>().Add(order);

            // 7- Save To Database
            int result = await _unitOfWork.Complete();
            if (result <= 0) return null;

            return order;

        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            return await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
        }

        public async Task<Order> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderWithItemAndDeliveryMethodSpecifications(orderId, buyerEmail);
            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            return order;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderWithItemAndDeliveryMethodSpecifications(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }
    }
}
