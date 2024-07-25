using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.Repositories;
using OrderService.Services;
using Shared.Messaging;
using Shared.Models;

namespace OrderService;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        // Load RabbitMQ settings
        services.Configure<RabbitMQSettings>(Configuration.GetSection("RabbitMQ"));
        
        services.AddControllers();
        
        // Configure the DbContext with a PostgreSQL database
        services.AddDbContext<OrderDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        
        // Register Dependency Injection
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderService, Services.OrderService>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
