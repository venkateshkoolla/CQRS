using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;

namespace Ocas.Domestic.Apply.Api.Configuration
{
    public class PartnerSwaggerProcessor : IOperationProcessor
    {
        public bool Process(OperationProcessorContext context)
        {
            context.OperationDescription.Operation.Parameters.Add(
            new NSwag.OpenApiParameter
            {
                Name = "X-Partner",
                Kind = NSwag.OpenApiParameterKind.Header,
                IsRequired = false,
                Description = "Pass in the partner code.",
                Schema = new NJsonSchema.JsonSchema
                {
                    Type = NJsonSchema.JsonObjectType.String
                }
            });

            return true;
        }
    }
}
