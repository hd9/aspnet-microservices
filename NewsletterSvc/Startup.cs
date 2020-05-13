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
using NewsletterSvc.Repositories;
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

            services.AddControllers();
            services.AddRouting(x => x.LowercaseUrls = true);
            services.AddSingleton<IMongoClient>(x => db);
            services.AddTransient<INewsletterRepository, NewsletterRepository>();
            services.AddTransient<INewsletterSvc, Svc.NewsletterSvc>();

            services.AddMassTransit(x =>
            {
                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(c =>
                {
                    c.Host(cfg.MassTransit.Host);
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

        private MongoClient InitDb()
        {
            return new MongoClient(cfg.MongoDb.ConnectionString, cfg.MongoDb.Db, cfg.MongoDb.Collection);
        }
    }
}
