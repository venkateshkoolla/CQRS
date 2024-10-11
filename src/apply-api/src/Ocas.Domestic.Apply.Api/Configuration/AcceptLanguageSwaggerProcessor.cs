﻿using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using Ocas.Domestic.Apply.Core;

namespace Ocas.Domestic.Apply.Api.Configuration
{
    public class AcceptLanguageSwaggerProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            context.OperationDescription.Operation.Parameters.Add(
            new NSwag.OpenApiParameter
            {
                Name = "Accept-Language",
                Kind = NSwag.OpenApiParameterKind.Header,
                IsRequired = false,
                Description = "Pass in the locale.",
                Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String,
                    Default = Constants.Localization.FallbackLocalization
                }
            });

            return true;
        }
    }
}
