using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.API.Helpers;
using Talabat.BLL.Interfaces;
using Talabat.BLL.Repositories;
using Talabat.BLL.Services;

namespace Talabat.API.Extentions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IResponceCacheService, ResponceCacheService>();
            services.AddScoped<IPaymentService, PaymentService>();
            // Allow Dependency Injection for TokenServices
            services.AddScoped(typeof(ITokenService), typeof(TokenService));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            // Allow Dependency Injection for Repositories
            services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            // Allow Dependency Injection for Order
            services.AddScoped<IOrderService, OrderServices>();

            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddDataProtection();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count > 0)
                                                          .SelectMany(M => M.Value.Errors)
                                                          .Select(E => E.ErrorMessage).ToArray();

                    var responseMessage = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(responseMessage);
                };
            });

            return services; // Return the modified IServiceCollection
        }

    }
}
