using NotificationSvc.Infrastructure.Options;
using System;
using System.Threading.Tasks;
using Core;
using Core.Shared;
using MassTransit;
using Microsoft.Extensions.Configuration;
using NotificationSvc.Consumers;
using NotificationSvc.Infrastructure;
using NotificationSvc.Models;
using NotificationSvc.Services;

namespace NotificationSvc
{
    public class Startup
    {
        public async Task Run()
        {
            var cfg = InitOptions<AppConfig>();

            await InitMassTransit(cfg);

            Console.WriteLine("NotificationSvc running.\nPress any key to exit");
            await Task.Run(() => Console.ReadKey());
        }

        private async static Task InitMassTransit(AppConfig cfg)
        {
            var dbLogger = new DbLogger(cfg.ConnectionString);

            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(cfg.MassTransit.Host);
                sbc.ReceiveEndpoint(cfg.MassTransit.Queue, e =>
                {
                    e.Consumer<NewsletterSubscribedConsumer>();
                    e.Consumer<MailSender>(() => new MailSender(cfg.Mail, dbLogger));
                });
            });

            await bus.StartAsync();
        }

        private static T InitOptions<T>()
            where T : new()
        {
            var config = InitConfig();
            return config.Get<T>();
        }

        private static IConfigurationRoot InitConfig()
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var builder = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{env}.json", true, true)
                .AddEnvironmentVariables();

            return builder.Build();
        }
    }
}
