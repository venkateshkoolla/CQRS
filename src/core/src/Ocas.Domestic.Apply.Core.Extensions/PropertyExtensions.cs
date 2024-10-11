using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Ocas.Domestic.Apply.Core.Extensions
{
    public static class PropertyExtensions
    {
        public static void PropsFilter(this object o, IEnumerable<string> props)
        {
            var propsList = props?.ToList();

            if (o == null || props == null || !propsList.Any())
            {
                return;
            }

            var t = o.GetType();

            foreach (var p in t.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                // If we can't get & set the property then we must skip it
                if (!p.CanWrite || !p.CanRead) { continue; }

                var mget = p.GetGetMethod(false);
                var mset = p.GetSetMethod(false);

                // Get and set methods have to be public
                if (mget == null) { continue; }
                if (mset == null) { continue; }

                // Must be nullable
                var canBeNull = !p.PropertyType.IsValueType || (Nullable.GetUnderlyingType(p.PropertyType) != null);
                if (!canBeNull)
                {
                    continue;
                }

                // If present in the props array then do not modify value
                if (props.Any(s => s.Equals(p.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                // Null the property!
                p.SetValue(o, null);
            }
        }
    }
}
