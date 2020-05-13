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
            services.AddTransient<INotificationRepository>(x => new NotificationRepository(Configuration["ConnectionString"]));
            services.AddTransient<INotificationSvc, Svc.NotificationSvc>();

            var repo = new NotificationRepository(cfg.ConnectionString);

            services.AddMassTransit(x =>
            {
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
                {
                    c.UseHealthCheck(context);
                    c.Host(cfg.MassTransit.Host);
                    c.ReceiveEndpoint(cfg.MassTransit.Queue, e =>
                    {
                        e.Consumer<NewsletterSubscribedConsumer>();
                        e.Consumer(() => new MailSenderConsumer(cfg.Mail, repo));
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
