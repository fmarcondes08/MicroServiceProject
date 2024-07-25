using NotificationService.Services;
using Shared.Messaging;
using Shared.Models;

namespace NotificationService;

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
        
        // Register Dependency Injection
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        services.AddHostedService<NotificationHandler>();
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
