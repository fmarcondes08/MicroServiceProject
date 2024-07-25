using Microsoft.EntityFrameworkCore;
using UserService.Controllers.Data;
using UserService.Repositories;
using UserService.Services;

namespace UserService;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();
        
        // Configure the DbContext with a PostgreSQL database
        services.AddDbContext<UserDbContext>(options =>
            options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
        
        // Register Dependency Injection
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IUserService, Services.UserService>();
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
