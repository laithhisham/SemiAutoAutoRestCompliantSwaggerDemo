﻿// <copyright file="SetBodyNameExtension.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Azure.AgPlatform.BaseNetCoreApp.ServiceCollectionExtentions.Helpers
{
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <summary>
    /// Updates the body parameter's name.
    /// </summary>
    public class SetBodyNameExtension : IRequestBodyFilter
    {
        /// <inheritdoc/>
        public void Apply(OpenApiRequestBody requestBody, RequestBodyFilterContext context)
        {
            var parameterInfo = context?.BodyParameterDescription?.ParameterInfo();

            if (parameterInfo != null)
            {
                requestBody?.Extensions.Add("x-bodyName", new OpenApiString(parameterInfo.Name));
            }
        }
    }
}
