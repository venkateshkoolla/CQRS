using System;
using System.Data;
using Dapper;

namespace Ocas.Domestic.Coltrane.Bds.Provider
{
    // https://stackoverflow.com/a/39730278
    public class DateTimeHandler : SqlMapper.TypeHandler<DateTime>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime value)
        {
            parameter.Value = value;
        }

        public override DateTime Parse(object value)
        {
            return DateTime.SpecifyKind((DateTime)value, DateTimeKind.Utc);
        }
    }
}
