using System;
using System.Collections.Generic;
using System.Data;
using Dapper;

namespace Ocas.Domestic.Crm.Extras.Provider.Extensions
{
    public static class TableTypeExtensions
    {
        public static SqlMapper.ICustomQueryParameter UniqueIdentifierListParameter(this Guid id)
        {
            return UniqueIdentifierListParameter(new List<Guid> { id });
        }

        public static SqlMapper.ICustomQueryParameter UniqueIdentifierListParameter(this IEnumerable<Guid> ids)
        {
            var idTable = new DataTable();
            idTable.Columns.Add("Id", typeof(Guid));
            ids = ids ?? new List<Guid>();

            foreach (var id in ids)
            {
                var row = idTable.NewRow();
                row[0] = id;
                idTable.Rows.Add(row);
            }

            idTable.EndLoadData();

            return idTable.AsTableValuedParameter("udtt_UniqueIdentifierList");
        }
    }
}
