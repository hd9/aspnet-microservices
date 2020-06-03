using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using Web.Infrastructure.Global;
using Web.Infrastructure.Options;
using Web.Services;

namespace Web
{
    public class Startup
    {

        #region Attributes
        public IConfiguration Configuration { get; }
        readonly AppConfig cfg;
        #endregion

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            cfg = configuration.Get<AppConfig>();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddRouting(x => x.LowercaseUrls = true);
            services.AddHttpClient<IRecommendationProxy, RecommendationProxy>();
            services.AddHttpClient<INewsletterProxy, NewsletterProxy>();
            services.AddHttpClient<ICatalogProxy, CatalogProxy>();
            services.AddHttpClient<IAccountProxy, AccountProxy>();
            services.AddHttpClient<IOrderProxy, OrderProxy>();

            Site.StoreSettings = cfg.StoreSettings;

            services.AddStackExchangeRedisCache(o =>
            {
                o.Configuration = cfg.Redis.Configuration;
                o.InstanceName = cfg.Redis.InstanceName;
            });

            //services.AddSession(o =>
            //{
            //    o.Cookie.Name = ".hildenco.session";
            //    o.IdleTimeout = TimeSpan.FromMinutes(10);
            //    o.Cookie.HttpOnly = true;
            //    o.Cookie.IsEssential = true;
            //    o.Cookie.SameSite = SameSiteMode.Strict;
            //});

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(o =>
                {
                    o.LoginPath = "/Account/SignIn";
                    o.AccessDeniedPath = "/Account/Forbidden";
                    o.Cookie.Name = ".hildenco.session";
                });

            // allow auto-rebuilding the cshtml after changes (dev-only)
            // https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-compilation?view=aspnetcore-3.0#runtime-compilation
            #if DEBUG
            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            #endif
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
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            //app.UseHttpContextItemsMiddleware();
            app.UseStaticFiles();
            //app.UseCookiePolicy();
            //app.UseSession();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
