﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public Task<IList<StudyArea>> GetStudyAreas(Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.StudyAreasGet.Locale, (int)locale }
            };

            return Connection.QueryAsync<StudyArea>(Sprocs.StudyAreasGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout).QueryToListAsync();
        }

        public Task<StudyArea> GetStudyArea(Guid id, Locale locale)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.StudyAreasGet.Id, id },
                { Sprocs.StudyAreasGet.Locale, (int)locale }
            };

            return Connection.QueryFirstOrDefaultAsync<StudyArea>(Sprocs.StudyAreasGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
