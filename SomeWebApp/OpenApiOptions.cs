using Microsoft.Azure.Global.Services.Common.Service.OpenApi.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;

namespace SomeWebApp
{
    internal class OpenApiOptions
    {
        public static Configuration GetConfiguration()
        {
            var info = new OpenApiDocumentInfo(Title, Description, Version, IdwClientName);
            return new Configuration(
              info,
              GetXmlCommentFileNames(),
              GetParameters(),
              PolymorphicTypes());
        }

        private static IList<Type> PolymorphicTypes()
        {
            return new List<Type> { };
        }

        private static Dictionary<string, OpenApiParameter> GetParameters()
        {
            return new Dictionary<string, OpenApiParameter>();
        }

        private static IList<string> GetXmlCommentFileNames()
        {
            return new List<string> { "SomeWebApp.xml" };
        }

        internal const string Title = "Security Insights";
        internal const string Description = "API spec for Microsoft.SecurityInsights (Azure Security Insights) resource provider";
        internal const string Version = "2021-09-01-preview";

        internal const string IdwClientName = "SecurityInsightsClient";
        internal const string JsonRoute = "api-docs/{documentName}/swagger.json";
        internal static string UiEndpoint()
        {
            return $"/api-docs/{Version}/swagger.json";
        }
    }
}