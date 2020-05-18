using CatalogSvc.Consumers;
using CatalogSvc.Infrastructure;
using CatalogSvc.Infrastructure.Options;
using CatalogSvc.Repositories;
using CatalogSvc.Services;
using Core.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Svc = CatalogSvc.Services;

namespace CatalogSvc
{
    public class Startup
    {

        public IConfiguration Configuration { get; }
        private readonly AppConfig cfg;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            cfg = configuration.Get<AppConfig>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var db = new MongoClient(cfg.Mongo);

            services.AddControllers();
            services.AddRouting(x => x.LowercaseUrls = true);
            services.AddSingleton<IMongoClient>(x => db);
            services.AddTransient<ICatalogRepository, CatalogRepository>();
            services.AddTransient<ICatalogSvc, Svc.CatalogSvc>();

            services.AddMassTransit(x =>
            {
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
                {
                    c.Host(cfg.MassTransit.Host);
                    c.ReceiveEndpoint(Global.Endpoints.ProductInfo, e =>
                    {
                        e.Consumer(() => new ProductInfoRequestConsumer(
                            context.Container.GetService<ICatalogSvc>()));
                    });
                }));
            });

            services.AddMassTransitHostedService();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            logger.LogInformation($"DB Settings: {cfg.Mongo}");
        }
    }
}
