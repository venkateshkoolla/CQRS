using System;
using Microsoft.Xrm.Sdk;

namespace Ocas.Domestic.Data.Mappers
{
    public static class EntityReferenceMapper
    {
        public static EntityReference ToEntityReference(this Guid source, string logicalName)
        {
            return new EntityReference(logicalName, source);
        }

        public static EntityReference ToEntityReference(this Guid? source, string logicalName)
        {
            return !source.HasValue ? null : new EntityReference(logicalName, source.Value);
        }
    }
}
