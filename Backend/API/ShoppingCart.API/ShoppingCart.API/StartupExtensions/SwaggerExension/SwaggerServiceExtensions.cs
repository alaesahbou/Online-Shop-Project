﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using ShoppingCart.API.Models;
using System.IO;
using System.Linq;
using System.Text;
using ShoppingCart.API.StartupExtensions.SwaggerExension.Filters;

namespace ShoppingCart.API
{
    /// <summary>
    /// Summary:
    ///    Represents the configuration definitions and usage of Swagger UI documentation
    /// </summary>
    public static class SwaggerServiceExtensions
    {
        internal const string AUTHORIZATION_SCHEME_NAME = "Bearer";

        /// <summary>
        /// Apply configuration for Swagger API documentation
        /// </summary>
        /// <param name="services">Service reference</param>
        /// <returns>Updated configuration of the service reference</returns>
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ShoppingCart Web API", Version = "v1" });
                /**
                 * Helps swagger identify the controller based on versioning without conflicts.
                 * Error Details: Actions require a unique method/path combination for Swagger/OpenAPI 3.0
                 * Solution: Call ResolveConflictingActions.
                 */
                c.ResolveConflictingActions(apiDescription => apiDescription.First());

                /*
                 * Add custom name to controllers for displaying in the Swagger documentation.
                 * Group name provided in the ApiExplorerSettings attribute will be mapped.
                 */
                c.TagActionsBy(apiDescriptionRef => new[] { apiDescriptionRef.GroupName });
                c.DocInclusionPredicate((groupName, apiDescriptionRef) => true);

                /**
                 * How to hide a property just in post request description of swagger using swashbuckle?
                 * Step 1: Install NuGet package: Swashbuckle.AspNetCore.Annotations
                 * Step 2: Call EnableAnnotations method detailed below
                 * Step 3: Use the attribute 'SwaggerSchema to ReadOnly' in the property that needs to be hidden.
                 */
                c.EnableAnnotations();
                /**
                 * Map the autogenerated xml file which contains the description of each implementation to the Swagger documentation.
                 */
                c.IncludeXmlComments(Path.Combine(System.AppContext.BaseDirectory, "ShoppingCart.API.xml"));
                c.IncludeXmlComments(AboutModelLibrary.XMLDocumentationFilePath);

                /**
                 * Adds a green padlock icon at the top of the Swagger UI documentation to include the JWT token.
                 * This defintion will be used by OperationFilter to apply popup at individual endpoints.
                 */
                StringBuilder strBuilderSecurityDesc = new StringBuilder();
                strBuilderSecurityDesc.AppendLine("JWT Authorization header using the Bearer scheme using the JSON format below<br>");
                strBuilderSecurityDesc.AppendLine("{<br>");
                strBuilderSecurityDesc.AppendLine("     Authorization: Bearer `JWT Token`<br>");
                strBuilderSecurityDesc.AppendLine("}");

                c.AddSecurityDefinition(AUTHORIZATION_SCHEME_NAME,
                    new OpenApiSecurityScheme
                    {
                        Description = strBuilderSecurityDesc.ToString(),
                        Type = SecuritySchemeType.Http, //We set the scheme type to http since we're using bearer authentication
                        Scheme = "bearer", //The name of the HTTP Authorization scheme to be used in the Authorization header. In this case "bearer".
                    });;

                //IOperationFilter is used to apply padlock icon only for endpoints with attributes - Microsoft.AspNetCore.Authorization.AuthorizeAttribute
                c.OperationFilter<AuthResponsesOperationFilter>();

                c.OperationFilter<FileUploadFilter>();
            });
            return services;
        }

        /// <summary>
        /// Extension method that applies the Swagger UI configuration.
        /// </summary>
        /// <param name="app">Application builder reference</param>
        /// <returns>Updated configuration of the Application builder reference</returns>
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ShoppingCart.API v1"));
            return app;
        }
    }
}
