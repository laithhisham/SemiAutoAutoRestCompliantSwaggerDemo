using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.Global.Services.Common.Service.OpenApi;
using Microsoft.Azure.AgPlatform.BaseNetCoreApp.ServiceCollectionExtentions.Helpers;
using Microsoft.Azure.Global.Services.Common.Service.OpenApi.DocumentFilters;
using Microsoft.Azure.Global.Services.Common.Service.OpenApi.SchemaFilters;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Azure.Global.Services.Common.Service.OpenApi.OperationFilters;
using Microsoft.Azure.AgPlatform.ResourceProviderService.Filters;

namespace SomeWebApp
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

            //services.AddOpenApiService(OpenApiOptions.GetConfiguration());
            var config = new
            {
                PolymorphicSchemaModels = new List<Type> { typeof(WeatherForecast) },
                ModelEnumsAsString = true,
                ReusableParameters = new Dictionary<string, Microsoft.OpenApi.Models.OpenApiParameter>()
                {
                    { "SubscriptionIdParameter", ArmReusableParameters.GetSubscriptionIdParameter() },
                    { "ResourceGroupNameParameter", ArmReusableParameters.GetResourceGroupNameParameter() },
                    { "ApiVersionParameter", ArmReusableParameters.GetApiVersionParameter() },
                    //{ "FarmBeatsResourceNameParameter", ArmReusableParameters.GetFarmBeatsResourceNameParameterParameter() },
                    //{ "ExtensionIdParameter", ArmReusableParameters.GetExtensionIdParameter() },
                },
                CommonDefintionModels = new List<string>()
                {
                    "WeatherForecast",
                    "ErrorAdditionalInfo",
                    "ErrorDetail",
                    "ErrorResponse",
                    "Operation",
                    "OperationDisplay",
                    "OperationListResponse",
                    "ProxyResource",
                    "Resource",
                    "SystemData",
                    "TrackedResource",
                },
                CommonParameterModels = new List<string>()
                {
                    "ApiVersionParameter",
                    "ResourceGroupNameParameter",
                    "SubscriptionIdParameter",
                },
                EnableSwaggerSecurityTokenSupport = true,
                XmlCommentFile = "SomeWebApp.xml"
            };

            services.AddSwaggerGen(c =>
            {
            //c.AddOptions(OpenApiOptions.GetConfiguration());

            c.SwaggerDoc(OpenApiOptions.GetConfiguration().GetInfo().Version, OpenApiOptions.GetConfiguration().GetInfo());
            // Setting type of IFormFile as "file" in swagger.
            c.MapType(typeof(IFormFile), () => new OpenApiSchema() { Type = "file", Format = "binary" });
            c.MapType(typeof(Stream), () => new OpenApiSchema() { Type = "file", Format = "binary" });
            c.CustomOperationIds(d => (d.ActionDescriptor as ControllerActionDescriptor)?.ActionName);

            c.IncludeXmlComments(config.XmlCommentFile, true);
            c.CustomSchemaIds(DefaultSchemaIdSelector);
            c.EnableAnnotations();

            // Set the body name correctly
            c.RequestBodyFilter<SetBodyNameExtension>();

            // This is used to remove duplicated api-version query parameter
            c.OperationFilter<RemoveDuplicateApiVersionParameterFilter>();

            // This is used to add the x-ms-long-running-operation attribute and the options
            c.OperationFilter<LongRunningOperationFilter>();

            // OpenAPI capability list documentation: https://msazure.visualstudio.com/One/_git/AGE-Documents?path=%2Fdocs%2FCommon%2FswaggerGenerationLibrary.md&_a=preview
            // Refer above before adding any new SchemaFilter/DocumentFilter, if not present, then can go via adding that in the library and then consuming.
            // Can check individual filers/documents added below for more info.
            c.DocumentFilter<PolymorphismDocumentFilter>(config.PolymorphicSchemaModels);

            // Schema level filters
            // Works in conjunction with PolymorphismDocumentFilter and PolymorphismSchemaFilter for GeoJsonObject like requirement.
            c.SchemaFilter<SubTypeOfFilter>();

            // Adds 'x-ms-azure-resource' extension to a class marked by Microsoft.Azure.Global.Services.Common.Service.OpenApi.Extensions.AzureResourceAttribute.
            c.SchemaFilter<AddAzureResourceExtentionFilter>();

            // Adds x-ms-enum to a property enum type. Adds extension attributes to indicate
            // AutoRest to model enum as string. This is as per OpenAPI specifications.
            c.SchemaFilter<AddEnumExtensionFilter>(config.ModelEnumsAsString);

            // Sets reusable properties like subscriptionId, resourceGroupName like attributes.
            c.DocumentFilter<SetCustomParametersFilter>(config.ReusableParameters);

            // Sets refs for reusable properties like subscriptionId, resourceGroupName like attributes.
            c.OperationFilter<Microsoft.Azure.AgPlatform.BaseNetCoreApp.ServiceCollectionExtentions.Helpers.FormatParametersFilter>(config.ReusableParameters);

            // Adds x-ms-mutability to the property marked by Microsoft.Azure.Global.Services.Common.Service.OpenApi.Extensions.MutabilityAttribute
            c.SchemaFilter<AddMutabilityExtentionFilter>();

            // Marked class will be flattened in client library by AutoRest to make it more user friendly.
            c.SchemaFilter<AddClientFlattenExtensionFilter>();

            // Adds "readOnly": true to the property marked by Microsoft.Azure.Global.Services.Common.Service.OpenApi.ValidationAttribute.ReadOnlyPropertyAttribute
            c.SchemaFilter<AddReadOnlyPropertyFilter>();

            // PolymorphismSchemaFilter find all the derived classes of passed base classes and register their schema.
            c.SchemaFilter<PolymorphismSchemaFilter>(config.PolymorphicSchemaModels);

            // Operation level filters
            // Set Description field using the XMLDoc summary if absent and clear summary. By
            // default Swashbuckle uses remarks tag to populate operation description instead
            // of using summary tag.
            c.OperationFilter<MoveSummaryToDescriptionFilter>();

            // Adds x-ms-pageable extension to operation marked with Page-able attribute.
            c.OperationFilter<AddPageableExtensionFilter>();

            // Adds x-ms-long-running-operation extension to operation marked with Microsoft.Azure.Global.Services.Common.Service.OpenApi.Extensions.LongRunningOperationAttribute.
            c.OperationFilter<AddLongRunningOperationExtensionFilter>();

            // Clear all the supported mime type from response object. Supported mime type is
            // added at document level, with hard coded value of application/json.
            c.OperationFilter<SetProducesContentTypeFilter>();

            // Clear all consumed types except application/json.
            c.OperationFilter<SetConsumesContentTypeFilter>();

            // This is applied if swagger is generated using open api 3.0 spec, helps to fix bug in autorest tool.
            // No impact for swagger generated using 2.0 spec.
            c.OperationFilter<ArrayInQueryParametersFilter>();

            // This is used to set default values, specifically to denote the api version as required parameter.
            c.OperationFilter<SwaggerDefaultValuesFilter>();

            // This is used to skip APIs from documentation conditionally.
            c.DocumentFilter<HideInDocsFilter>();

            var conf = OpenApiOptions.GetConfiguration();
            c.AddSecurityRequirement(conf.GetOpenApiSecurityRequirement());
            c.AddSecurityDefinition("azure_auth", conf.GetAzureSecurityDefinition());
            // Globally enable security based on SwaggerConfig configuration.
            //if (config.EnableSwaggerSecurityTokenSupport)
            //{
            //        c.AddSecurityDefinition(, .GetAzureSecurityDefinition());
            //          ConstantsOpenApiSwagger.AzureAuthSecuritySchemeReferenceId,
            //          ConstantsOpenApiSwagger.AzureAuthOpenApiSecScheme);

            //        // Enable the security definition and requirement for Swagger.
            //        c.AddSecurityRequirement(ConstantsOpenApiSwagger.AzureAuthSecurityRequirement);


            //        if (config.Identity == SwaggerIdentity.ResourceProviderService)
            //    {
                       
            //        c.AddSecurityDefinition(
            //            ConstantsOpenApiSwagger.AzureAuthSecuritySchemeReferenceId,
            //            ConstantsOpenApiSwagger.AzureAuthOpenApiSecScheme);

            //        // Enable the security definition and requirement for Swagger.
            //        c.AddSecurityRequirement(ConstantsOpenApiSwagger.AzureAuthSecurityRequirement);
            //    }
            //    else
            //    {
            //        // First create the security definition for JWT tokens
            //        // The security type "SecuritySchemeType.ApiKey" needs to be used as long as Swagger v2.0 is used
            //        // As soon as Swagger v3.0 is used, the Swagger JSON definition supports the http-scheme with Bearer tokens and will generate correctly.
            //        // More details can be found here:
            //        // - https://swagger.io/docs/specification/2-0/authentication/
            //        // - https://swagger.io/docs/specification/authentication/
            //        c.AddSecurityDefinition(
            //            ConstantsOpenApiSwagger.BearerSecuritySchemeReferenceId,
            //            ConstantsOpenApiSwagger.BearerOpenApiSecScheme);

            //        // Enable the security definition and requirement for Swagger.
            //        c.AddSecurityRequirement(ConstantsOpenApiSwagger.BearerSecurityRequirement);
            //    }


            });
            services.AddSwaggerGenNewtonsoftSupport();
        }

        private static string DefaultSchemaIdSelector(Type modelType)
        {
            if (!modelType.IsConstructedGenericType)
            {
                return modelType.Name;
            }

            var prefix = modelType.GetGenericArguments()
                .Select(genericArg => DefaultSchemaIdSelector(genericArg))
                .Aggregate((previous, current) => previous + current);

            return prefix + modelType.Name.Split('`').First();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger(option =>
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint.
                option.RouteTemplate = OpenApiOptions.JsonRoute;
                // Change generated swagger version to 2.0
                option.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint(OpenApiOptions.UiEndpoint(), OpenApiOptions.Description);
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
