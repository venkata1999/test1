using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;

namespace HSBC.Deposits.Personnel.Vehicle
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

            services.AddApiVersioning(
                o =>
                {
                    o.DefaultApiVersion = new ApiVersion(1, 0);
                    o.AssumeDefaultVersionWhenUnspecified = true;
                }
                );

            //Swagger Start 
            //TO Enable Swagger in WebAPI
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Integration-API(s)",
                    Description = ""
                });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "AAD B2cAuthorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Common Service API Auth"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });

                c.EnableAnnotations();
            });
            //Swagger End
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //Swagger Start 
            //TO Enable Swagger in WebAPI
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            //var BaseURL = Configuration["BaseURL-kv"].Replace("{port}", Configuration["ServicePort"]);
            //app.UseSwagger(x =>
            //{
            //    x.PreSerializeFilters.Add((swagger, httpReq) =>
            //    {
            //        swagger.Servers = BaseURL.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(url => new OpenApiServer { Url = $"{httpReq.Scheme}://{url.Trim()}" }).ToList();
            //    });
            //});

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Integration API V1");

            });
        }
    }
}
