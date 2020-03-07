using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SwashbuckleExample
{
    public class Startup
    {
        private const string OPEN_API_SPECIFICATION_NAME = "open-api-spec";
        private const string API_NAME = "The API";

        private string _openApiSpecAddress = $"/swagger/{OPEN_API_SPECIFICATION_NAME}/swagger.json";
        private string _uiAddress = $"index.html";


        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
            });

            services.AddControllers();

            services.AddSwaggerGen(setupAction =>
            {
                setupAction.SwaggerDoc(OPEN_API_SPECIFICATION_NAME,
                    new Microsoft.OpenApi.Models.OpenApiInfo()
                    {
                        Title = API_NAME,
                        Version = "1",
                        Description = "Through this API you can access books, authors and the weather forecast :)",
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact
                        {
                            Email = "john.doe@email.com",
                            Name = "John Doe",
                            Url = new Uri("https://my.homepage.com")
                        },
                        License = new Microsoft.OpenApi.Models.OpenApiLicense
                        {
                            Name = "MIT License",
                            Url = new Uri("https://opensource.org/licenses/MIT")
                        }
                    });

                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });
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

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                setupAction.SwaggerEndpoint(
                    _openApiSpecAddress,
                    API_NAME);
                setupAction.RoutePrefix = string.Empty;
            });
        }
    }
}
