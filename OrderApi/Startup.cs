using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EventSourcing.Library;
using EventSourcing.Library.Infrastructure;
using EventSourcing.Library.Utilities;
using Pak.ApplicationServices;
using Pak.ReadModel;

namespace OrderApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddScoped<IAggregateFactory, AggregateFactory>();
            services.AddScoped(x => new SqlServerEventStoreConfig
            {
                ConnectionString = Configuration.GetConnectionString("EVENT_STORE_DB")
            });
            services.AddScoped<IRepository, SqlServerRepository>();
            services.AddScoped<OrderApplicationService>();
            services.AddScoped<PackageApplicationService>();

            // Read Model
            var connectionString = Configuration.GetConnectionString("PROJECTIONS_DB");
            services.AddSingleton<IProjectionWriter>(x => new SqlServerProjectionWriter(connectionString));
            services.AddSingleton<IEventDispatcher, EventDispatcher>();
            services.AddSingleton<ObserverRegistry>();
            services.AddSingleton<OrdersObserver>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Prepare Read Model
            var serviceProvider = app.ApplicationServices;
            var registry = serviceProvider.Resolve<ObserverRegistry>();
            registry.Register();
        }
    }
}
