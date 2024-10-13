using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using Talabat.API.Errors;
using Talabat.API.Extensions;
using Talabat.API.Extentions;
using Talabat.API.Helpers;
using Talabat.API.MiddleWares;
using Talabat.BLL.Interfaces;
using Talabat.BLL.Repositories;
using Talabat.BLL.Services;
using Talabat.DAL.Data;
using Talabat.DAL.Entities.Identity;
using Talabat.DAL.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Asp Web APIs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Allow Dependency Injection for StoreContext
builder.Services.AddDbContext<StoreContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddDbContext<AppIdentityDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
});

// Allow Dependency Injection for Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(S =>
{
    var connection = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
    return ConnectionMultiplexer.Connect(connection);
});

//// Allow Dependency Injection for TokenServices
//builder.Services.AddScoped(typeof(ITokenService), typeof(TokenService));

//// Allow Dependency Injection for Repositories
//builder.Services.AddScoped(typeof(IBasketRepository), typeof(BasketRepository));
//builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//// Allow Dependency Injection for Order
//builder.Services.AddScoped(typeof(IOrderService), typeof(OrderServices));

//builder.Services.AddAutoMapper(typeof(MappingProfiles));

//builder.Services.Configure<ApiBehaviorOptions>(options =>
//{
//    options.InvalidModelStateResponseFactory = actionContext =>
//    {
//        var errors = actionContext.ModelState.Where(M => M.Value.Errors.Count > 0)
//                                             .SelectMany(M => M.Value.Errors)
//                                             .Select(E => E.ErrorMessage).ToArray();

//        var responseMessage = new ApiValidationErrorResponse()
//        {
//            Errors = errors
//        };
//        return new BadRequestObjectResult(responseMessage);
//    };
//});
builder.Services.AddApplicationServices();
//public IConfiguration Configuration { get; }

// Add Identity Services
builder.Services.AddIdentityServices(builder.Configuration);


builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
        //policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("URL Here"); 
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleWare>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseCors("CorsPolicy");


app.UseAuthentication(); // Ensure authentication middleware is used
app.UseAuthorization();

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var loggerFactory = services.GetRequiredService<ILoggerFactory>();

try
{
    var context = services.GetRequiredService<StoreContext>();
    await context.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context, loggerFactory);

    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
    await identityContext.Database.MigrateAsync();

    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
}
catch (Exception ex)
{
    var logger = loggerFactory.CreateLogger<Program>();
    logger.LogError(ex, "An error occurred during migration");
}

app.Run();
