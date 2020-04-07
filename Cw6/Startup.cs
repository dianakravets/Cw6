using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw6.Middlewares;
using Cw6.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace Cw6
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
            services.AddTransient<IDbservice, Dbservice>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env,IDbservice dbservice)
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
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseMiddleware<LoggingMiddleware>();
             app.Use(async (context, next) =>
             {
                 if (!context.Request.Headers.ContainsKey("Index"))
                 {
                     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                     await context.Response.WriteAsync("Podaj index");
                     return;
                 }

                 var index = context.Request.Headers["Index"].ToString();
                 if (!dbservice.ckeckIndex(index))
                 {
                     context.Response.StatusCode = StatusCodes.Status404NotFound;
                     await context.Response.WriteAsync("Nie istnieje taki student");
                     return;
                 }
                 if(dbservice.ckeckIndex(index))
                 {
                     context.Response.StatusCode = StatusCodes.Status200OK;
                     await context.Response.WriteAsync("Student o takim indeksie jest w bazie");
                     return;
                 }
                 
                 await next();
             });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}