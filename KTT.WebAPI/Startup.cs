using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KTT.WebAPI.Models;
using KTT.WebAPI.Services;
using KTT.WebAPI.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Configuration;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace KTT.WebAPI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddMvc(option => option.EnableEndpointRouting = false);
            

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "KTT Web API", Version = "v1" });
            });

            ConfigureJwt(services);

            RegisterDIs(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "KTT Web API");
            });


            app.UseExceptionHandler(
               options =>
               {
                   options.Run(async context =>
                   {

                       context.Response.StatusCode = 500;//Internal Server Error
                       context.Response.ContentType = "application/json";
                       await context.Response.WriteAsync("We are working on it. ");
                       var ex = context.Features.Get<IExceptionHandlerFeature>();
                       if (ex != null)
                       {
                           await context.Response.WriteAsync($"Error: {ex.Error.Message}");
                       }
                   });
               }
               );

            app.UseAuthentication();
            app.UseMvc();
        }

        #region Helpers

        private void ConfigureJwt(IServiceCollection services)
        {
            // Configure Jwt authentication
            var secret = configuration.GetSection("AppSettings").GetValue<string>("Secret");
            var key = Encoding.ASCII.GetBytes(secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience =false,
                };
            });
        }

        private void RegisterDIs(IServiceCollection services)
        {
            // DI registration
            services.AddSingleton<IEntityMaint<Product>, EntityMaint<Product>>();
            services.AddSingleton<IEntityMaint<User>, EntityMaint<User>>();
            services.AddSingleton<IJwtTokenManager, JwtTokenManager>();
        }

        #endregion

    }
}
