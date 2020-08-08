using AutoMapper;
using Core.Handlers;
using Kantin.Data;
using Kantin.Extensions;
using Kantin.Handler;
using Kantin.Service.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using Azure.Storage.Blobs;
using Core.Helpers;
using Core.Interface;
using Microsoft.AspNetCore.Http.Features;

namespace Kantin
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
            services.AddControllers()
                .AddNewtonsoftJson(opt => {
                    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    opt.SerializerSettings.ContractResolver = new JsonContractResolver();
                });

            // Add framework services.
            var connectionString = Configuration.GetConnectionString("SqlConnection");
            services.AddDbContext<KantinEntities>(options => 
            {
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Kantin"));
            });

            // Add Validation services
            services.AddTransient<ITokenAuthorizationService, TokenAuthorizationService>();
            services.AddJWTAuthentication();

            // Add File services
            services.AddSingleton<IFileStorage<BlobContainerClient>, FileStorageHelper>();

            // Add AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Add Swagger 
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Kantin API", Version = "v1" });
                c.SchemaFilter<SwaggerFilter>();
            });

            services.Configure<FormOptions>(o => {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, KantinEntities dbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                dbContext.Database.Migrate();
            }

            ConfigureSwaggerApp(app);

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private void ConfigureSwaggerApp(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Kantin API V1"));
        }
    }
}
