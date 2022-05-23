using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using JavnaNabava.Models;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using JavnaNabava.Controllers;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;

namespace JavnaNabava
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var appSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSection);

            services.AddDbContext<RPPP23Context>(options => options.UseSqlServer(Configuration.GetConnectionString("RPPP23")));

            services.AddControllersWithViews()
                    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>()).AddJsonOptions(configure => configure.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddTransient<DrzaveController>();
            services.AddTransient<VrstKompsController>();
            services.AddTransient<GodineController>();
            services.AddTransient<KriterijiController>();
            services.AddTransient<OvlasteniciController>();
            services.AddTransient<GradoviController>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "REST API - Javna Nabava - V1", Version = "v1" });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "RPPP JavnaNabava WebAPI");
                c.RoutePrefix = "swagger";
            });

            app.UseRouting();
            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {

               endpoints.MapDefaultControllerRoute();
            });

        }
    }
}