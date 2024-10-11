using System;

namespace Ocas.Domestic.Data.Utils
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(this Guid g)
        {
            return g == Guid.Empty;
        }

        public static bool IsEmpty(this Guid? g)
        {
            return !g.HasValue || g.Value.IsEmpty();
        }
    }
}
