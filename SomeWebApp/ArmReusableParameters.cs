﻿// <copyright file="ArmReusableParameters.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Azure.AgPlatform.ResourceProviderService.Filters
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;

    /// <summary>
    /// Arm reusable parameters list.
    /// </summary>
    internal static class ArmReusableParameters
    {
        /// <summary>
        /// GetSubscriptionIdParameter.
        /// </summary>
        /// <returns>OpenApiParameter for subscriptionId.</returns>
        internal static OpenApiParameter GetSubscriptionIdParameter()
        {
            return new OpenApiParameter
            {
                Description = "The Azure subscription ID. This is a GUID-formatted string " +
                "(e.g. 00000000-0000-0000-0000-000000000000).",
                Name = "subscriptionId",
                In = ParameterLocation.Path,
                Required = true,
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    MinLength = 1,
                },
            };
        }

        /// <summary>
        /// GetResourceGroupNameParameter.
        /// </summary>
        /// <returns>OpenApiParameter for resourceGroupName.</returns>
        internal static OpenApiParameter GetResourceGroupNameParameter()
        {
            OpenApiParameter resourceGroupNameParameter = new OpenApiParameter
            {
                Description = "The name of the resource group. The name is case insensitive.",
                Name = "resourceGroupName",
                In = ParameterLocation.Path,
                Required = true,
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    Pattern = "^[-\\w\\._\\(\\)]+$",
                    MinLength = 1,
                    MaxLength = 90,
                },
            };

            resourceGroupNameParameter.Extensions.Add("x-ms-parameter-location", new OpenApiString("method"));
            return resourceGroupNameParameter;
        }

        /// <summary>
        /// GetApiVersionParameter.
        /// </summary>
        /// <returns>OpenApiParameter for api-version.</returns>
        internal static OpenApiParameter GetApiVersionParameter()
        {
            return new OpenApiParameter
            {
                Description = "The API version to be used with the HTTP request.",
                Name = "api-version",
                In = ParameterLocation.Query,
                Required = true,
                Schema = new OpenApiSchema()
                {
                    Type = "string",
                    MinLength = 1,
                },
            };
        }

        ///// <summary>
        ///// GetFarmBeatsResourceNameParameterParameter.
        ///// </summary>
        ///// <returns>OpenApiParameter for farmBeatsResourceName.</returns>
        //internal static OpenApiParameter GetFarmBeatsResourceNameParameterParameter()
        //{
        //    OpenApiParameter farmBeatsResourceNameParameter = new OpenApiParameter
        //    {
        //        Description = "FarmBeats resource name.",
        //        Name = "farmBeatsResourceName",
        //        In = ParameterLocation.Path,
        //        Required = true,
        //        Schema = new OpenApiSchema() { Type = "string" },
        //    };

        //    farmBeatsResourceNameParameter.Extensions.Add("x-ms-parameter-location", new OpenApiString("method"));
        //    return farmBeatsResourceNameParameter;
        //}

        ///// <summary>
        ///// GetExtensionIdParameter.
        ///// </summary>
        ///// <returns>OpenApiParameter for extensionId.</returns>
        //internal static OpenApiParameter GetExtensionIdParameter()
        //{
        //    OpenApiParameter extensionIdParameter = new OpenApiParameter
        //    {
        //        Description = "Id of extension resource.",
        //        Name = "extensionId",
        //        In = ParameterLocation.Path,
        //        Required = true,
        //        Schema = new OpenApiSchema() { Type = "string" },
        //    };

        //    extensionIdParameter.Extensions.Add("x-ms-parameter-location", new OpenApiString("method"));
        //    return extensionIdParameter;
        //}
    }
}
