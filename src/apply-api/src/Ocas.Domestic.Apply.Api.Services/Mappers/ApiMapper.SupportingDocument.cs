using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ocas.Domestic.Apply.Core;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public IList<SupportingDocument> MapSupportingDocuments(IList<Dto.SupportingDocument> list, IList<LookupItem> supportingDocumentTypes, IList<LookupItem> officials, IList<LookupItem> institutes)
        {
            return list.Select(dtoDoc =>
            {
                var docType = supportingDocumentTypes.FirstOrDefault(t => t.Id == dtoDoc.DocumentTypeId);
                var docOfficial = officials.FirstOrDefault(o => o.Id == dtoDoc.OfficialId);
                var docInstitute = institutes.FirstOrDefault(i => i.Id == dtoDoc.InstituteId);

                var doc = _mapper.Map<SupportingDocument>(dtoDoc);

                if (Constants.SupportingDocumentTypes.CustomizeName.Any(c => c.Equals(docType?.Code)))
                {
                    doc.Name = CustomizeDocumentName(docOfficial, docType, docInstitute);
                }

                if (docType?.Code == Constants.SupportingDocumentTypes.EvaluationReport)
                {
                    doc.Type = SupportingDocumentType.IntlCredentialAssessment;
                }

                return doc;
            }).ToList();
        }

        public IList<SupportingDocument> MapSupportingDocuments(IList<Dto.Transcript> transcripts, IList<College> colleges, IList<University> universities)
        {
            foreach (var transcript in transcripts)
            {
                if (transcript?.TranscriptType == TranscriptType.OntarioCollegeUniversityTranscript)
                {
                    var college = colleges.FirstOrDefault(x => x.Id == transcript.PartnerId);

                    var university = universities.FirstOrDefault(x => x.Id == transcript.PartnerId);

                    transcript.Name = college?.Name ?? university?.Name;
                }
            }

            return _mapper.Map<IList<SupportingDocument>>(transcripts);
        }

        public IList<SupportingDocument> MapSupportingDocuments(IList<Dto.Test> tests, IList<LookupItem> testTypes)
        {
            var supportingDocuments = _mapper.Map<IList<SupportingDocument>>(tests);

            foreach (var test in tests)
            {
                supportingDocuments.First(x => x.Id == test.Id).Name = testTypes.FirstOrDefault(x => x.Id == test.TestTypeId)?.Label + " (" + test.DateTestTaken.ToStringOrDefault() + ")";
            }

            return supportingDocuments;
        }

        public IList<SupportingDocument> MapSupportingDocuments(IList<Dto.AcademicRecord> academicRecords)
        {
            return _mapper.Map<IList<SupportingDocument>>(academicRecords);
        }

        private string CustomizeDocumentName(LookupItem official, LookupItem docType, LookupItem institute)
        {
            var sb = new StringBuilder();
            if (official != null) sb.Append(official.Label).Append(' ');
            if (docType != null) sb.Append(docType.Label).Append(' ');
            if (institute != null) sb.Append("- ").Append(institute.Label);

            return sb.ToString().Trim();
        }
    }
}
