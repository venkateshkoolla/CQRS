using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.AppSettings.Extras;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public ApplicationCycle MapApplicationCycle(Dto.ApplicationCycle cycle, LookupItem appCycleStatus, DateTime considerationDate)
        {
            var model = _mapper.Map<ApplicationCycle>(cycle);
            model.Status = appCycleStatus.Code;
            model.EqualConsiderationDate = considerationDate;
            return model;
        }

        public IList<ApplicationCycle> MapApplicationCycles(IList<Dto.ApplicationCycle> list, IList<LookupItem> applicationCycleStatuses, IAppSettingsExtras appSettingsExtras)
        {
            var applicationCycles = new List<ApplicationCycle>();

            foreach (var item in list)
            {
                var appCycleStatus = applicationCycleStatuses.First(x => x.Id == item.StatusId);
                var model = MapApplicationCycle(item, appCycleStatus, appSettingsExtras.GetApplicationCycleEqualConsiderationDate(item.StartDate.Year));
                applicationCycles.Add(model);
            }

            return applicationCycles;
        }
    }
}
