using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Stripe;
using Talabat.API.Errors;
using Talabat.BLL.Interfaces;
using Talabat.DAL.Entities;
using Talabat.DAL.Entities.Order_Aggregate;

namespace Talabat.API.Controllers
{
    public class PaymentsController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger logger;
        private const string _whSecret = "whsec_dd70e942eb013777159f8699e94349ee5fa08c60910ff034be7be299541ae3db";

        public PaymentsController(IPaymentService paymentService , ILogger<PaymentsController> logger)
        {
            _paymentService = paymentService;
            this.logger = logger;
        }

        [Authorize]
        [HttpPost("{basketId}")] //api/payments/{basketId}
        // response for creating or updating a payment intent for a given basket
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            
            if (basket is null) return BadRequest(new ApiResponse(400,"Proplem With Your Basket"));
           
            return Ok(basket);
        }

        [HttpPost("webhook")] // api/payments/webhook
        // handle incoming webhook events from Stripe
        public async Task<ActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], _whSecret);

                PaymentIntent intent;
                Order order;

                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        logger.LogInformation("Payment Succeeded");
                        intent = stripeEvent.Data.Object as PaymentIntent;

                        
                        order = await _paymentService.UpdateOrderPaymentStatus(intent.Id, OrderStatus.PaymentReceived);
                        break;

                    case Events.PaymentIntentPaymentFailed:
                        intent = stripeEvent.Data.Object as PaymentIntent;

                        order = await _paymentService.UpdateOrderPaymentStatus(intent.Id, OrderStatus.PaymentFailed);
                        logger.LogInformation("Payment Failed for Order:" , order.Id);
                        logger.LogInformation("Payment Failed for Intent:", intent.Id);
                        break;
                }

                return new EmptyResult();
            }
            catch (StripeException e)
            {
                return BadRequest(new ApiResponse(400, e.Message));
            }
        }





    }
}
