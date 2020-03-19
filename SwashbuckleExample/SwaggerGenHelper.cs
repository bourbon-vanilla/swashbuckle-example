using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;

using System.Linq;


namespace SwashbuckleExample
{
    internal static class SwaggerGenHelper
    {

        public static bool AddControllerMethodToVersionedSwaggerDocumentPredicate(string documentName, ApiDescription apiDescription)
        {
            var actionApiVersionModel = apiDescription.ActionDescriptor
                .GetApiVersionModel(ApiVersionMapping.Explicit | ApiVersionMapping.Implicit);

            if (actionApiVersionModel == null)
                return true;

            if (actionApiVersionModel.DeclaredApiVersions.Any())
                return actionApiVersionModel.DeclaredApiVersions.Any(apiVersion =>
                    GetVersionedApiName(apiVersion) == documentName);

            return actionApiVersionModel.ImplementedApiVersions.Any(apiVersion =>
                GetVersionedApiName(apiVersion) == documentName);
        }

        public static string GetVersionedApiName(ApiVersion version)
        {
            return string.Format(
                $"book-library-api-specification-v{{0}}", version);
        }

    }
}
