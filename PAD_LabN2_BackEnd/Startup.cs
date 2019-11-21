using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PAD_LabN2_BackEnd.Services;
using PAD_LabN2_BackEnd.Services.Consumer;
using PadLabN2_BackEnd.Controllers;

namespace PAD_LabN2_BackEnd
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSingleton<IProducerService, ProducerService>();
            services.AddSingleton<IConsumerService, ConsumerService>();
            services.AddSingleton<MessageHub>();
            services.AddCors(options =>
                options.AddPolicy("AllowAny", builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }));
            services.AddSignalR();
        }
        
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowAny");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
            app.UseStatusCodePages();
            app.UseSignalR(routes => { routes.MapHub<MessageHub>("/hub"); });
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}