using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Bara.Core.Mapper;
using Bara.Sample.Web.Business;
using Bara.Sample.Web.DataAccess;
using Microsoft.Extensions.Logging;

namespace Bara.Sample.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            ConfigureDomainServices(services);
            ConfigureDataAccess(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory factory)
        {
            factory.AddDebug();
            factory.AddConsole();
            //factory.AddNLog();
            //factory.ConfigureNLog("Nlog.config");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        public void ConfigureDomainServices(IServiceCollection services)
        {
            services.AddSingleton<HomeService>();
        }

        public void ConfigureDataAccess(IServiceCollection services)
        {
            services.AddSingleton(sp =>
            {
                var loggerfactory = sp.GetRequiredService<ILoggerFactory>();
                return MapperContainer.Instance.GetBaraMapper(loggerfactory, "BaraMapConfig.xml");
            });
            services.AddSingleton<MoviesDataAccess>();
        }
    }
}
