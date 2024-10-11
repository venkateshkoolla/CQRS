using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Ocas.Domestic.Apply.Core.Extensions
{
    public static class ObjectToDictionaryExtensions
    {
        public static IDictionary<string, object> ToDictionary(this object source, IDictionary<string, string> propsToLocale)
        {
            return source.ToDictionary<object>(propsToLocale);
        }

        private static IDictionary<string, T> ToDictionary<T>(this object source, IDictionary<string, string> propsToLocale)
        {
            if (source == null)
            {
                new ArgumentNullException(nameof(source), "Unable to convert object to a dictionary. The source object is null.");
            }

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                var name = property.Name;
                if (propsToLocale.TryGetValue(property.Name, out var nameValue))
                {
                    name = nameValue;
                }

                AddPropertyToDictionary(property, name, source, dictionary);
            }

            return dictionary;
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, string propertyKey, object source, Dictionary<string, T> dictionary)
        {
            var value = property.GetValue(source) ?? string.Empty;

            if (IsOfType<T>(value))
            {
                dictionary.Add(propertyKey, (T)value);
            }
        }

        private static bool IsOfType<T>(object value)
        {
            return value is T;
        }
    }
}
