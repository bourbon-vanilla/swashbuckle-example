using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;


namespace SwashbuckleExample
{
    internal static class SwaggerGenOptionsExtensions
    {

        public static void SetupSwagger(this SwaggerGenOptions options, IServiceCollection temporaryServiceCollection)
        {
            options.AddVersionedSwaggerDocuments(
                temporaryServiceCollection);

            options.AddSecurity();

            options.DocInclusionPredicate(
                SwaggerGenHelper.AddControllerMethodToVersionedSwaggerDocumentPredicate);

            options.IncludeXmlComments(
                GetAssemblyXmlCommentsAbsolutePath());
        }


        #region PRIVATE MEHODS

        private static void AddVersionedSwaggerDocuments(this SwaggerGenOptions options, IServiceCollection temporaryServiceCollection)
        {
            var apiVersionDescriptionProvider = temporaryServiceCollection.BuildServiceProvider()
            .GetService<IApiVersionDescriptionProvider>();

            foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                options.AddSwaggerDocument(apiVersionDescription.ApiVersion, "The Library API");
        }

        private static void AddSwaggerDocument(this SwaggerGenOptions setupAction,
            ApiVersion apiVersion, string apiName)
        {
            setupAction.SwaggerDoc(SwaggerGenHelper.GetVersionedApiName(apiVersion),
                new OpenApiInfo()
                {
                    Title = apiName,
                    Version = apiVersion.ToString(),
                    Description = "Through this API you can access books",
                    Contact = new OpenApiContact
                    {
                        Email = "john.doe@email.com",
                        Name = "John Doe",
                        Url = new Uri("https://my.homepage.com")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
        }

        private static void AddSecurity(this SwaggerGenOptions options)
        {
            var authName = "myBasicAuth";

            options.AddSecurityDefinition(authName, new OpenApiSecurityScheme()
            {
                Type = SecuritySchemeType.Http,
                Scheme = "basic",
                Description = "Input your username and password to access this API"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = authName
                            }
                        }, new List<string>()
                    }
                });
        }

        private static string GetAssemblyXmlCommentsAbsolutePath()
        {
            var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            return Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
        }

        #endregion

    }
}
