using CatalogSvc.Infrastructure;
using CatalogSvc.Services;
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

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var db = InitDb();

            services.AddControllers();
            services.AddRouting(x => x.LowercaseUrls = true);
            services.AddSingleton<IMongoClient>(x => db);
            services.AddSingleton<ICatalogSvc>(x => new Svc.CatalogSvc(db));
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
            var cs = Configuration["DbSettings:ConnStr"];
            var db = Configuration["DbSettings:Db"];
            var c = Configuration["DbSettings:Collection"];

            return new MongoClient(cs, db, c);
        }
    }
}
