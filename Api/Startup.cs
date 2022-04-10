using Api.Authorization;
using Api.Injections;
using Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Service;
using Service.Account;

namespace Api
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

          
            var jwtSettings = Configuration.GetSection("Jwt").Get<JwtSettings>();

            services.AddDatabaseDeveloperPageExceptionFilter();

          
            services.AddDomainLayer(Configuration);
            services.AddServiceLayer();

            services.Configure<JwtSettings>(Configuration.GetSection("Jwt"));

            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddControllers();

            services.AddSwagger();
         
            services.AddApiVersion();

            services.AddAuth(jwtSettings);
            services.AddCors();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(options =>
            {
                options.WithOrigins("https://localhost:44374")
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuth();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}
