using AutoMapper;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using SwashbuckleExample.Authentication;
using SwashbuckleExample.Database;
using SwashbuckleExample.Dto;
using SwashbuckleExample.Entities;

using System;


namespace SwashbuckleExample
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
            services.AddMvc(setupAction =>
            {
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError));
                setupAction.Filters.Add(
                    new ProducesDefaultResponseTypeAttribute());
                setupAction.Filters.Add(
                    new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized));
                setupAction.Filters.Add(
                    new AuthorizeFilter());

                setupAction.ReturnHttpNotAcceptable = true;
            });

            services.AddControllers();

            services.AddDbContext<BookLibraryContext>(optionsBuilder => 
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString()));

            services.AddLocalServices();

            services.AddAutoMapper(configAction => 
            {
                configAction.CreateMap<Author, AuthorDto>();
                configAction.CreateMap<AuthorDto, Author>();
                configAction.CreateMap<Book, BookDto>();
                configAction.CreateMap<BookDto, Book>();
            }, typeof(Startup));

            services.AddAuthentication("Basic")
                .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("Basic", null);

            services.AddVersionedApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = "'v'VV";
            });

            services.AddApiVersioning(setupAction => 
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ReportApiVersions = true;
            });

            services.AddSwaggerGen(options => 
                options.SetupSwagger(services));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(setupAction =>
            {
                foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
                    setupAction.SwaggerEndpoint(
                        $"/swagger/{SwaggerGenHelper.GetVersionedApiName(description.ApiVersion)}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                setupAction.RoutePrefix = string.Empty;
            });

        }

    }
}
