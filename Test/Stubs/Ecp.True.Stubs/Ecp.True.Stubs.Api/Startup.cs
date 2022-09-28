using Ecp.True.Stubs.Api.Models;
using Ecp.True.Stubs.Api.Processor;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Ecp.True.Stubs.Api
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
            RegisterServices(services, Configuration);
            services.AddApplicationInsightsTelemetry();
            services.AddMvc(option => option.EnableEndpointRouting = false).AddNewtonsoftJson().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMiddleware(typeof(RequestHandler));
            app.UseMvc();
        }

        private static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IOwnershipResponseProcessor, OwnershipResponseProcessor>();
            services.Configure<StubConfiguration>(configuration.GetSection("ConnectionInfo"));
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });
        }
    }
}
