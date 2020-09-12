using Banking.Net.Command.Transactions.Application.Contracts;
using Banking.Net.Command.Transactions.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;

namespace Banking.API
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
            try
            {
                services.AddCors();
                services.AddControllers();
                services.AddTransient<ITransactionApplicationService, TransactionApplicationService>();
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Banking API",
                        Version = "v1",
                        Description = "RESTful API for Banking",
                        Contact = new OpenApiContact()
                        {
                            Name = "Efrain Bautista",
                            Email = ""
                        }
                    });
                    c.TagActionsBy(api => new[] { api.GroupName });
                    c.DocInclusionPredicate((name, api) => true);
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            app.UseEndpoints(endpoints =>
                endpoints.MapControllers()
            );
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "BANKING API V1");
            });
        }
    }
}