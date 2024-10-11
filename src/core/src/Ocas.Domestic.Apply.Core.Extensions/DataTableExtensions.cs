using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ocas.Domestic.Apply.Core.Extensions
{
    public static class DataTableExtensions
    {
        public static DataTable ToDataTable(this List<IDictionary<string, object>> list)
        {
            var result = new DataTable();
            if (list.Count == 0)
                return result;

            var columnNames = list.SelectMany(dict => dict.Keys).Distinct();
            result.Columns.AddRange(columnNames.Select(c => new DataColumn(c)).ToArray());
            foreach (var item in list)
            {
                var row = result.NewRow();
                foreach (var key in item.Keys)
                {
                    row[key] = item[key];
                }

                result.Rows.Add(row);
            }

            return result;
        }
    }
}
