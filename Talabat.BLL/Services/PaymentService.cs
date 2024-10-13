using Microsoft.Extensions.Configuration;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.BLL.Interfaces;
using Talabat.BLL.Specifications.Order_Specifications;
using Talabat.DAL.Entities;
using Talabat.DAL.Entities.Order_Aggregate;
using Product = Talabat.DAL.Entities.Product;

namespace Talabat.BLL.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PaymentService(IConfiguration configuration,
            IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _configuration = configuration;
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }

        //create or update a payment intent for a given basket
        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            //// Set the Stripe API key from the configuration settings
            StripeConfiguration.ApiKey = _configuration["StripeSettings:Secretkey"];

            var basket = await _basketRepository.GetCustomerBasket(basketId);



            if (basket is null) return null;

            var shippingPrice = 0m;

            // If the basket has a delivery method
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                // get the cost of the delivery method
                shippingPrice = deliveryMethod.Cost;
            }

            foreach (var item in basket.Items)
            {
                var product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                // If the price differs from the product's current price
                if (item.Price != product.Price)
                    //update it
                    item.Price = product.Price;
            }


            var service = new PaymentIntentService();

            PaymentIntent intent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                //creating a new payment intent
                var options = new PaymentIntentCreateOptions
                {
                    Amount = (long)basket.Items.Sum(i => i.Quantity * (i.Price * 100)) + ((long)shippingPrice * 100),
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                intent = await service.CreateAsync(options);
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                // If the payment intent ID is exist
                var options = new PaymentIntentUpdateOptions
                {
                    //update the payment intent
                    Amount = (long)basket.Items.Sum(i => (i.Quantity * (i.Price * 100))) + (long)(shippingPrice * 100)
                };

                await service.UpdateAsync(basket.PaymentIntentId, options);

            }
            basket.ShippingPrice = shippingPrice;
            await _basketRepository.UpdateCustomerBasket(basket);

            return basket;
        }

        //public async Task<Order> UpdateOrderPaymentSucceded(string paymantIntentId)
        //{
        //    var spec = new OrderWithItemByPaymentIntentSpecifications(paymantIntentId);

        //    var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

        //    if (order is null) return null;

        //    order.Status = OrderStatus.PaymentReceived;

        //    _unitOfWork.Repository<Order>().Update(order);

        //    await _unitOfWork.Complete();
        //    return order;
        //}

        //public async Task<Order> UpdateOrderPaymentFailed(string paymantIntentId)
        //{
        //    var spec = new OrderWithItemByPaymentIntentSpecifications(paymantIntentId);

        //    var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

        //    if (order is null) return null;

        //    order.Status = OrderStatus.PaymentFailed;

        //    _unitOfWork.Repository<Order>().Update(order);

        //    await _unitOfWork.Complete();
        //    return order;
        //}

        public async Task<Order> UpdateOrderPaymentStatus(string paymentIntentId, OrderStatus status)
        {
            
            var spec = new OrderWithItemByPaymentIntentSpecifications(paymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetByIdWithSpecAsync(spec);

            if (order is null) return null;

            order.Status = status;

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.Complete();

            return order;
        }

    }
}
