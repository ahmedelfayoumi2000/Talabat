using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Talabat.API.Erro;

namespace Talabat.API.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddleWare> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
        {
            this.next = next;
            this.logger = logger;
            this.env = env;
        }
        // Invoked to handle requests
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context); 
            }
            catch (Exception ex)
            {

                logger.LogError(ex ,ex.Message);

                context.Response.ContentType = "application/json";
                // Sets the response status code to 500 Internal Server Error
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                // Creates a response message
                var responcceMessage = env.IsDevelopment()
                    ? new ApiExceptionResponce((int)HttpStatusCode.InternalServerError , ex.Message , ex.StackTrace.ToString()) 
                    : new ApiExceptionResponce((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString());

                // Configures JSON serialization options to use camelCase naming
                var options = new JsonSerializerOptions()
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                // Serializes the response message to JSON
                var json = JsonSerializer.Serialize(responcceMessage , options);
                await context.Response.WriteAsync(json);
            }

        }
    }
}
