using RecommendationSvc.Repositories;
using RecommendationSvc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Svc = RecommendationSvc.Services;
using MassTransit;
using RecommendationSvc.Infrastructure.Options;
using RecommendationSvc.Consumers;
using Core.Commands.Catalog;
using System;
using Core.Infrastructure;

namespace RecommendationSvc
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
            services.AddControllers();
            services.AddRouting(x => x.LowercaseUrls = true);
            services.AddTransient<IRecommendationSvc, Svc.RecommendationSvc>();
            services.AddTransient<IRecommendationRepository>(x => new RecommendationRepository(cfg.ConnectionString));

            services.AddMassTransit(x =>
            {
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
                {
                    c.Host(cfg.MassTransit.Host);
                    c.ReceiveEndpoint(cfg.MassTransit.Queue, e =>
                    {
                        e.Consumer(() => new OrderSubmittedConsumer(
                            context.Container.GetService<IRecommendationSvc>()));
                    });
                }));

                x.AddRequestClient<ProductInfoRequest>(
                    new Uri($"{cfg.MassTransit.Host}/{Global.Endpoints.ProductInfo}"));
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

            logger.LogInformation($"Connection String: {Configuration["ConnectionString"]}");
        }
    }
}
