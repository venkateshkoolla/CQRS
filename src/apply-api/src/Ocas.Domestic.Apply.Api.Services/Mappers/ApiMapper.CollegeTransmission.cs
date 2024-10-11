using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public CollegeTransmission MapCollegeTransmission(DateTime? lastLoadDateTime, Dto.ProgramChoice choice)
        {
            return new CollegeTransmission
            {
                ApplicationId = choice.ApplicationId,
                ContextId = choice.Id,
                CollegeId = choice.CollegeId.GetValueOrDefault(),
                Sent = lastLoadDateTime,
                Type = CollegeTransmissionType.ProgramChoice,
                WaitingForPayment = choice.SupplementalFeePaid == false
            };
        }

        public CollegeTransmission MapCollegeTransmission(Guid applicationId, DateTime? lastLoadDateTime, Dto.Education education, Guid collegeId)
        {
            return new CollegeTransmission
            {
                ApplicationId = applicationId,
                CollegeId = collegeId,
                ContextId = education.Id,
                Sent = lastLoadDateTime,
                Type = CollegeTransmissionType.Education
            };
        }

        public CollegeTransmission MapCollegeTransmission(DateTime? lastLoadDateTime, Dto.Offer offer)
        {
            return new CollegeTransmission
            {
                ApplicationId = offer.ApplicationId,
                CollegeId = offer.CollegeId,
                ContextId = offer.Id,
                Type = CollegeTransmissionType.Offer,
                Sent = lastLoadDateTime
            };
        }

        public CollegeTransmission MapCollegeTransmission(
            Guid applicationId,
            DateTime? lastLoadDateTime,
            Dto.SupportingDocument supportingDocument,
            Guid collegeId,
            bool requiredToSend,
            IList<LookupItem> supportingDocumentTypes,
            IList<LookupItem> officials,
            IList<LookupItem> institutes)
        {
            var docType = supportingDocumentTypes.FirstOrDefault(t => t.Id == supportingDocument.DocumentTypeId);
            var docOfficial = officials.FirstOrDefault(o => o.Id == supportingDocument.OfficialId);
            var docInstitute = institutes.FirstOrDefault(i => i.Id == supportingDocument.InstituteId);

            var doc = new CollegeTransmission
            {
                ApplicationId = applicationId,
                CollegeId = collegeId,
                ContextId = supportingDocument.Id,
                Name = supportingDocument.Name,
                RequiredToSend = requiredToSend,
                Sent = lastLoadDateTime,
                Type = CollegeTransmissionType.SupportingDocument
            };

            if (Constants.SupportingDocumentTypes.CustomizeName.Any(c => c.Equals(docType?.Code)))
            {
                doc.Name = CustomizeDocumentName(docOfficial, docType, docInstitute);
            }

            return doc;
        }

        public CollegeTransmission MapCollegeTransmission(Guid applicationId, DateTime? lastloadDateTime, Dto.Test test, Guid collegeId, IList<LookupItem> testTypes)
        {
            return new CollegeTransmission
            {
                ApplicationId = applicationId,
                CollegeId = collegeId,
                ContextId = test.Id,
                Name = $"{testTypes.FirstOrDefault(x => x.Id == test.TestTypeId)?.Label} ({test.DateTestTaken.ToStringOrDefault()})",
                Sent = lastloadDateTime,
                Type = CollegeTransmissionType.SupportingDocument
            };
        }

        public CollegeTransmission MapCollegeTransmission(Guid applicationId, DateTime? lastLoadDateTime, Guid collegeId, Guid academicRecordId, string name)
        {
            return new CollegeTransmission
            {
                ApplicationId = applicationId,
                CollegeId = collegeId,
                ContextId = academicRecordId,
                Name = name,
                Sent = lastLoadDateTime,
                Type = CollegeTransmissionType.SupportingDocument
            };
        }
    }
}
