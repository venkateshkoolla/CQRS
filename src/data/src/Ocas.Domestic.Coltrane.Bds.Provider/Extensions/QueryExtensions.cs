using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ocas.Domestic.Coltrane.Bds.Provider
{
    public static class QueryExtensions
    {
        public static async Task<IList<T>> QueryToListAsync<T>(this Task<IEnumerable<T>> queryResult)
        {
            var result = await queryResult;

            return result as IList<T> ?? result.ToList();
        }
    }
}
