using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial class DomesticContext
    {
        public Task<AcademicRecord> GetAcademicRecord(Guid id)
        {
            return CrmExtrasProvider.GetAcademicRecord(id);
        }

        public Task<IList<AcademicRecord>> GetAcademicRecords(Guid applicantId)
        {
            return CrmExtrasProvider.GetAcademicRecords(applicantId);
        }

        public async Task<AcademicRecord> CreateAcademicRecord(AcademicRecordBase academicRecordBase)
        {
            var crmEntity = CrmMapper.MapAcademicRecordBase(academicRecordBase);
            var id = await CrmProvider.CreateEntity(crmEntity);

            return await GetAcademicRecord(id);
        }

        public Task DeleteAcademicRecord(Guid academicRecordId)
        {
            return CrmProvider.DeactivateEntity(Crm.Entities.ocaslr_academicdata.EntityLogicalName, academicRecordId);
        }

        public async Task<AcademicRecord> UpdateAcademicRecord(AcademicRecord academicRecord)
        {
            var crmEntity = CrmProvider.AcademicData.FirstOrDefault(x => x.Id == academicRecord.Id) ?? throw new Exception($"AcademicData {academicRecord.Id} does not exist");

            CrmMapper.PatchAcademicRecord(academicRecord, crmEntity);
            await CrmProvider.UpdateEntity(crmEntity);

            return await GetAcademicRecord(crmEntity.Id);
        }
    }
}
