using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Global.Services.Common.Service.OpenApi;
using Microsoft.Azure.AgPlatform.BaseNetCoreApp.ServiceCollectionExtentions.Helpers;

namespace SomeWebApp
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
            services.AddControllers();

            //services.AddOpenApiService(OpenApiOptions.GetConfiguration());

            services.AddSwaggerGen(options =>
            {
                options.AddOptions(OpenApiOptions.GetConfiguration());
                options.DocumentFilter<HideInDocsFilter>();
            });
            services.AddSwaggerGenNewtonsoftSupport();
            //services.Add(OpenApiOptions.GetConfiguration());
            // services.AddSwaggerGen(c =>
            //{
            //    //c.EnableAnnotations(enableAnnotationsForInheritance: true, enableAnnotationsForPolymorphism: true);
            //    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(option=>
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                option.RouteTemplate = OpenApiOptions.JsonRoute;
                // Change generated swagger version to 2.0
                option.SerializeAsV2 = true;                  
            });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(OpenApiOptions.UiEndpoint(), OpenApiOptions.Description);                
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
