using AccountSvc.Consumers;
using AccountSvc.Infrastructure.Options;
using AccountSvc.Repositories;
using AccountSvc.Services;
using Microservices.Core.Infrastructure.Options;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Svc = AccountSvc.Services;
using GreenPipes;

namespace AccountSvc
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
            services.AddScoped<IAccountSvc, Svc.AccountSvc>();
            services.AddScoped<IAccountRepository>(x => new AccountRepository(cfg.ConnectionString));
            services.AddSingleton(cfg.EmailTemplates);

            services.AddMassTransit(x =>
            {
                x.AddConsumer<NewsletterSubscribedConsumer>();
                x.AddConsumer<AccountInfoRequestConsumer>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
                {
                    c.Host(cfg.MassTransit.Host);
                    c.ReceiveEndpoint(cfg.MassTransit.Queue, e =>
                    {
                        e.PrefetchCount = 16;
                        e.UseMessageRetry(r => r.Interval(2, 3000));
                        e.ConfigureConsumer<NewsletterSubscribedConsumer>(context);
                        e.ConfigureConsumer<AccountInfoRequestConsumer>(context);
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

            logger.LogInformation($"Connection String: {cfg.ConnectionString}");
        }
    }
}
