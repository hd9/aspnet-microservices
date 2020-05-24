using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NotificationSvc.Consumers;
using NotificationSvc.Infrastructure.Options;
using NotificationSvc.Repositories;
using NotificationSvc.Services;
using Svc = NotificationSvc.Services;
using GreenPipes;
using HildenCo.Core.Infrastructure.Options;

namespace NotificationSvc
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
            services.AddScoped<INotificationRepository>(x => new NotificationRepository(Configuration["ConnectionString"]));
            services.AddScoped<INotificationSvc, Svc.NotificationSvc>();
            services.AddSingleton(cfg.SmtpOptions);

            var repo = new NotificationRepository(cfg.ConnectionString);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<SendMailConsumer>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
                {
                    c.Host(cfg.MassTransit.Host);
                    c.ReceiveEndpoint(cfg.MassTransit.Queue, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(2, 100));
                        e.ConfigureConsumer<SendMailConsumer>(context);
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

            logger.LogInformation($"Connection String: {Configuration["DbSettings:ConnStr"]}, Db: {Configuration["DbSettings:Db"]}, Collection: {Configuration["DbSettings:Collection"]}");
        }

    }
}
