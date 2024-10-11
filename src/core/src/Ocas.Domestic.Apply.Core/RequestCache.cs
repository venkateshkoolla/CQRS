using System.Collections.Generic;

namespace Ocas.Domestic.Apply
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2237:Mark ISerializable types with serializable", Justification = "Not needed to be serializable")]
    public class RequestCache : Dictionary<string, object>
    {
        public T Get<T>()
          where T : class
        {
            var type = typeof(T);

            return Get<T>(type.Name);
        }

        public T Get<T>(string key)
            where T : class
        {
            if (TryGetValue(key, out var myObject))
            {
                return myObject as T;
            }

            return default(T);
        }

        public void AddOrUpdate<T>(T value)
        {
            var type = typeof(T);

            this[type.Name] = value;
        }

        public void AddOrUpdate<T>(string key, T value)
        {
            this[key] = value;
        }
    }
}
