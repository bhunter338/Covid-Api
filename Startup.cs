using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Covid_Api.Data;
using Covid_Api.ScheduledTask;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Covid_Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IWebHostEnvironment _env;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                  {
                      options.AddPolicy("CorsPolicy",
                          builder => builder
                              .AllowAnyMethod()
                              .AllowCredentials()
                              .SetIsOriginAllowed((host) => true)
                              .AllowAnyHeader());
                  });


            var conStrBuilder = new SqlConnectionStringBuilder(Configuration.GetConnectionString("CovidDbConnection"));
            // conStrBuilder.UserID = Configuration["CovidDb:SqlServerUsername"];
            // conStrBuilder.Password = Configuration["CovidDb:SqlServerPassword"];

            services.AddDbContext<CovidAppContext>(opt => opt.UseSqlServer(conStrBuilder.ConnectionString));

            services.AddControllers();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<IHostedService, InsertService>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Covid_Api", Version = "v1" });
            });
            // services.AddScoped<ICovidDataRepo, CovidDataRepo>();
            services.AddScoped<ICovidDataRepo, SqlCovidRepo>();
            // services.AddScoped<IServiceProvider, ServiceProvider>();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Covid_Api v1"));
            }
            app.UseCors("CorsPolicy");
            //test


            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
