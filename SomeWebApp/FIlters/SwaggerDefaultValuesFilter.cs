﻿// <copyright file="SwaggerDefaultValuesFilter.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Azure.AgPlatform.BaseNetCoreApp.ServiceCollectionExtentions.Helpers
{
    using System;
    using System.Linq;
    //using Microsoft.AspNetCore.Mvc.ApiExplorer;
    //using Microsoft.Azure.AgPlatform.BaseNetCoreApp.Config.Interfaces;
    //using Microsoft.Azure.AgPlatform.Common.Utilities;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Represents the Swagger/Swashbuckle operation filter used to document the implicit API version parameter.
    /// </summary>
    public class SwaggerDefaultValuesFilter : IOperationFilter
    {
        //private readonly IApiConfig config;

        /// <summary>
        /// Initializes a new instance of the <see cref="SwaggerDefaultValuesFilter"/> class.
        /// </summary>
        /// <param name="config">API config.</param>
        public SwaggerDefaultValuesFilter(/*IApiConfig config*/)
        {
            //this.config = config;
        }

        /// <inheritdoc/>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation is null)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var apiDescription = context.ApiDescription;

            //operation.Deprecated |= apiDescription.IsDeprecated();

            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters.OfType<OpenApiParameter>())
            {
                var description = apiDescription.ParameterDescriptions.FirstOrDefault(p => p.Name == parameter.Name);

                if (description != null)
                {
                    if (parameter.Description == null)
                    {
                        parameter.Description = description.ModelMetadata?.Description;
                    }

                    // Do not populate default value for api-version while specification generation is going on at build time.
                    // while populate for local testing.
                    if (true
                        //&& !this.config.IsSwaggerSpecGenerationInProgress
                        )
                    {
                        if (parameter.Schema.Default == null && description.DefaultValue != null)
                        {
                            parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                        }
                    }

                    parameter.Required |= description.IsRequired;
                }
            }
        }
    }
}
