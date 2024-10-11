using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Data
{
    public partial interface IDomesticContext
    {
        Task<AcademicRecord> CreateAcademicRecord(AcademicRecordBase academicRecordBase);
        Task DeleteAcademicRecord(Guid academicRecordId);
        Task<AcademicRecord> GetAcademicRecord(Guid id);
        Task<IList<AcademicRecord>> GetAcademicRecords(Guid applicantId);
        Task<AcademicRecord> UpdateAcademicRecord(AcademicRecord academicRecord);
    }
}
