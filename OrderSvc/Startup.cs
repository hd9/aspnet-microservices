using OrderSvc.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Svc = OrderSvc.Services;
using OrderSvc.Repositories;
using MassTransit;
using OrderSvc.Infrastructure.Options;
using OrderSvc.Consumers;
using GreenPipes;
using HildenCo.Core.Contracts.Account;
using System;

namespace OrderSvc
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
            services.AddScoped<IOrderSvc, Svc.OrderSvc>();
            services.AddScoped<IOrderRepository>(x => new OrderRepository(cfg.ConnectionString));
            services.AddSingleton(cfg.EmailTemplates);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<PaymentResponseConsumer>();
                x.AddConsumer<ShippingResponseConsumer>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
                {
                    c.Host(cfg.MassTransit.Host);
                    c.ReceiveEndpoint(cfg.MassTransit.Queue, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(n => n.Interval(2, 100));
                        e.ConfigureConsumer<PaymentResponseConsumer>(context);
                        e.ConfigureConsumer<ShippingResponseConsumer>(context);
                    });
                }));

                x.AddRequestClient<AccountInfoRequest>(
                    new Uri($"{cfg.MassTransit.Host}/account"));
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
