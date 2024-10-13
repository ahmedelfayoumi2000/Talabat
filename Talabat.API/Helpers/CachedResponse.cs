using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using System.Text;
using Talabat.BLL.Interfaces;

namespace Talabat.API.Helpers
{
    public class CachedResponse : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLifeInSeconds;

        public CachedResponse(int timeToLifeInSeconds)
        {
            _timeToLifeInSeconds = timeToLifeInSeconds;
            
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheServices = context.HttpContext.RequestServices.GetRequiredService<IResponceCacheService>();

            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
             
            var cachedResponced = await cacheServices.GetCachedResponce(cacheKey);

            if(!string.IsNullOrEmpty(cachedResponced))
            {
                var contectResult = new ContentResult()
                {
                    Content = cachedResponced,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contectResult;

                return;
            }

           var executedEndPointContext =  await next();

            if (executedEndPointContext.Result is OkObjectResult okObjectResult)
               await cacheServices.CacheResponceAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLifeInSeconds));
            
        }

        private string GenerateCacheKeyFromRequest(HttpRequest request)
         {
            var KeyBuilder = new StringBuilder();

            KeyBuilder.Append($"{request.Path}");  // /api/products

            foreach (var (key , value) in request.Query.OrderBy(X => X.Key))
            {
                KeyBuilder.Append($"|{key}-{value}");
            }
            return KeyBuilder.ToString();
        }
    }
}
