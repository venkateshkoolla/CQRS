using Microsoft.Xrm.Sdk;

namespace Ocas.Domestic.Crm.Provider
{
    internal static class EntityExtensions
    {
        internal static T GetAliasedAttributeValue<T>(this Entity entity, string attribute)
        {
            var attributeValue = entity.GetAttributeValue<AliasedValue>(attribute);
            if (attributeValue?.Value != null)
            {
                return (T)attributeValue.Value;
            }

            return default(T);
        }
    }
}
