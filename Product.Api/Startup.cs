using Product.DI;
using System.Linq;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Product.Api
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
            new DependencyResolver(Configuration).Resolve(services);

            services.AddSwaggerGen(x =>
            {
                x.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                x.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Product API",
                    Version = "v1",
                    Description = "Here you can create, delete product also list products and view special product."
                });
                x.CustomSchemaIds(s => s.FullName);
                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme { In = ParameterLocation.Header, Description = "Please enter JWT with Bearer into field", Name = "Authorization", Type = SecuritySchemeType.ApiKey });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "API V1");
            });

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
