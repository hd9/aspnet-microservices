using GreenPipes;
using HildenCo.Core.Contracts.Catalog;
using HildenCo.Core.Infrastructure;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RecommendationSvc.Consumers;
using RecommendationSvc.Infrastructure.Options;
using RecommendationSvc.Repositories;
using RecommendationSvc.Services;
using System;
using Svc = RecommendationSvc.Services;

namespace RecommendationSvc
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        readonly AppConfig cfg;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            cfg = configuration.Get<AppConfig>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRouting(x => x.LowercaseUrls = true);
            services.AddScoped<IRecommendationRepository>(x => new RecommendationRepository(cfg.ConnectionString));
            services.AddScoped<IRecommendationSvc, Svc.RecommendationSvc>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderSubmittedConsumer>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
                {
                    c.Host(cfg.MassTransit.Host);
                    c.ReceiveEndpoint(cfg.MassTransit.Queue, e =>
                    {
                        e.UseMessageRetry(n => n.Interval(2, 100));
                        e.ConfigureConsumer<OrderSubmittedConsumer>(context);
                    });
                }));

                x.AddRequestClient<ProductInfoRequest>(
                    new Uri($"{cfg.MassTransit.Host}/catalog"));
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
