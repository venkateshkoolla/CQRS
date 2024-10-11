using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<CollegeApplicationCycle> MapCollegeApplicationCycles(IList<Dto.CollegeApplicationCycle> list, IList<ApplicationCycle> applicationCycles, IList<LookupItem> applicationCycleStatuses)
        {
            var collegeApplicationCycles = new List<CollegeApplicationCycle>();
            foreach (var dbDto in list)
            {
                var applicationCycle = applicationCycles.FirstOrDefault(a => a.Id == dbDto.ApplicationCycleId);
                if (applicationCycle != null)
                {
                    var statusId = applicationCycleStatuses.FirstOrDefault(s => s.Code == applicationCycle.Status)?.Id ?? Guid.Empty;

                    collegeApplicationCycles.Add(new CollegeApplicationCycle
                    {
                        Id = dbDto.Id,
                        MasterId = applicationCycle.Id,
                        Year = applicationCycle.Year,
                        CollegeId = dbDto.CollegeId,
                        Name = dbDto.Name,
                        StartDate = applicationCycle.StartDate,
                        EndDate = applicationCycle.EndDate,
                        StatusId = statusId
                    });
                }
            }

            return collegeApplicationCycles;
        }
    }
}
