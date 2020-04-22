using Core.Shared;
using Core.Infrastructure.Options;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NewsletterSvc.Infrastructure;
using NewsletterSvc.Infrastructure.Options;
using NewsletterSvc.Services;
using System.Threading.Tasks;
using Svc = NewsletterSvc.Services;

namespace NewsletterSvc
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
            var db = InitDb();
            var bus = InitMassTransit();

            services.AddControllers();
            services.AddRouting(x => x.LowercaseUrls = true);
            services.AddSingleton<IMongoClient>(x => db);
            services.AddSingleton<IBusControl>(x => bus);
            services.AddSingleton<INewsletterSvc>(x => new Svc.NewsletterSvc(db, bus));
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

        private MongoClient InitDb()
        {
            return new MongoClient(cfg.MongoDb.ConnectionString, cfg.MongoDb.Db, cfg.MongoDb.Collection);
        }

        private IBusControl InitMassTransit()
        {
            //var host = "rabbitmq://localhost";

            //var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            //{
            //    cfg.Host(new Uri(host));
            //});

            //await bus.StartAsync();

            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host(cfg.MassTransit.Host);
            });

            bus.Start();

            return bus;
        }
    }
}
