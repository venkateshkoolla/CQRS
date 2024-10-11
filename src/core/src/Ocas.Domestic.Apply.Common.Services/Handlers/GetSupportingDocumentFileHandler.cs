using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Enums;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Templates;
using Ocas.Domestic.Apply.Services.Extensions;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Apply.TemplateProcessors;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Services.Handlers
{
    public class GetSupportingDocumentFileHandler : IRequestHandler<GetSupportingDocumentFile, BinaryDocument>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly IUserAuthorizationBase _userAuthorization;
        private readonly IRazorTemplateService _razorTemplateService;
        private readonly ILookupsCacheBase _lookupsCache;
        private readonly ITranslationsCache _translationsCache;
        private readonly ITemplateMapper _templateMapper;
        private readonly string _locale;

        public GetSupportingDocumentFileHandler(
            ILogger<GetSupportingDocumentFileHandler> logger,
            IDomesticContext domesticContext,
            IUserAuthorizationBase userAuthorization,
            IRazorTemplateService razorTemplateService,
            ILookupsCacheBase lookupsCache,
            ITranslationsCache translationsCache,
            ITemplateMapper templateMapper,
            RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _razorTemplateService = razorTemplateService ?? throw new ArgumentNullException(nameof(razorTemplateService));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _translationsCache = translationsCache ?? throw new ArgumentNullException(nameof(translationsCache));
            _templateMapper = templateMapper ?? throw new ArgumentNullException(nameof(templateMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<BinaryDocument> Handle(GetSupportingDocumentFile request, CancellationToken cancellationToken)
        {
            var transcript = await _domesticContext.GetTranscript(request.Id);
            if (transcript != null)
                return await HandlePostSecondaryTranscript(request, transcript);

            var academicRecord = await _domesticContext.GetAcademicRecord(request.Id);
            if (academicRecord != null)
                return await HandleAcademicRecord(request, academicRecord);

            var standardizedTest = await _domesticContext.GetTest(request.Id, _locale.ToLocaleEnum());
            if (standardizedTest != null)
                return await HandleStandardizedTest(request, standardizedTest);

            var supportingDocument = await _domesticContext.GetSupportingDocument(request.Id);
            if (supportingDocument != null)
                return await HandleOtherSupportingDocuments(request, supportingDocument);

            throw new NotFoundException($"No supporting document found {request.Id}");
        }

        private async Task<BinaryDocument> HandleAcademicRecord(GetSupportingDocumentFile request, Dto.AcademicRecord academicRecord)
        {
            await _userAuthorization.CanAccessApplicantAsync(request.User, academicRecord.ApplicantId);

            var transcripts = await _domesticContext.GetTranscripts(new Dto.GetTranscriptOptions { ContactId = academicRecord.ApplicantId });
            var grades = await _domesticContext.GetOntarioStudentCourseCredits(
                new Dto.GetOntarioStudentCourseCreditOptions
                {
                    ApplicantId = academicRecord.ApplicantId
                });

            var viewModel = _templateMapper.MapHighSchoolGrades(
                academicRecord,
                transcripts,
                grades,
                await _lookupsCache.GetHighSchools(_locale),
                await _lookupsCache.GetHighSkillsMajors(_locale),
                await _lookupsCache.GetHighestEducations(_locale),
                await _lookupsCache.GetLiteracyTests(_locale),
                await _lookupsCache.GetCommunityInvolvements(_locale),
                await _lookupsCache.GetCourseStatuses(_locale),
                await _lookupsCache.GetCourseTypes(_locale),
                await _lookupsCache.GetCourseDeliveries(_locale),
                await _lookupsCache.GetGradeTypes(_locale),
                _locale);

            var translationsDictionary = await _translationsCache.GetTranslations(_locale);
            viewModel.LoadTranslations(translationsDictionary);

            // finally generate pdf from razor template and ViewModel
            var pdf = await _razorTemplateService.GenerateHighSchoolGradesAsync(viewModel);

            return new BinaryDocument
            {
                Data = pdf,
                MimeType = Constants.MimeType.Pdf,
                CreatedDate = DateTime.UtcNow,
                CreatedBy = "OCAS",
                Name = $"{viewModel.Labels.OntarioHighSchoolTranscript}.pdf".SanitizeFileName()
            };
        }

        private async Task<BinaryDocument> HandleOtherSupportingDocuments(GetSupportingDocumentFile request, Dto.SupportingDocument supportingDocument)
        {
            if (supportingDocument.ApplicantId.IsEmpty())
            {
                throw new ValidationException($"No ApplicantId {supportingDocument.ApplicantId} on SupportingDocument {supportingDocument.Id}");
            }

            await _userAuthorization.CanAccessApplicantAsync(request.User, supportingDocument.ApplicantId.Value);

            var response = await _domesticContext.GetSupportingDocumentBinaryData(request.Id)
                           ?? throw new NotFoundException($"Binary data {request.Id} not found.");

            var isValidBase64 = IsValidBase64(response.DocumentBody, out var binaryData);
            if (!isValidBase64) throw new ValidationException("Invalid Base64");

            return new BinaryDocument
            {
                Data = binaryData,
                MimeType = response.MimeType,
                CreatedDate = response.CreatedOn,
                CreatedBy = "OCAS",
                Name = response.FileName.SanitizeFileName()
            };
        }

        private async Task<BinaryDocument> HandlePostSecondaryTranscript(GetSupportingDocumentFile request, Dto.Transcript transcript)
        {
            if (transcript.ContactId.IsEmpty())
            {
                throw new ValidationException($"No ContactId {transcript.ContactId} on Transcript {transcript.Id}");
            }

            await _userAuthorization.CanAccessApplicantAsync(request.User, transcript.ContactId.Value);

            if (transcript.TranscriptType != TranscriptType.OntarioCollegeUniversityTranscript)
            {
                throw new ValidationException($"Invalid Transcript type {transcript.TranscriptType}");
            }

            // get base64 encoded xml from AnnotationBase table
            var supportingDocumentBinaryData = await _domesticContext.GetSupportingDocumentBinaryData(request.Id) ?? throw new NotFoundException($"Binary data {request.Id} not found.");

            // convert base64 to byte[]
            var isValidBase64 = IsValidBase64(supportingDocumentBinaryData.DocumentBody, out var binaryData);
            if (!isValidBase64) throw new ValidationException("Invalid Base64");

            // transcript could be from ontario college or university
            var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
            var college = colleges.FirstOrDefault(x => x.Id == transcript.PartnerId);

            var universities = await _lookupsCache.GetUniversities();
            var university = universities.FirstOrDefault(x => x.Id == transcript.PartnerId);

            var instituteName = college?.Name ?? university?.Name;

            // create ViewModel for cshtml -> pdf generator
            var viewModel = new PostSecondaryTranscriptViewModel();
            var translationsDictionary = await _translationsCache.GetTranslations(_locale);
            viewModel.LoadTranslations(translationsDictionary, instituteName);

            using (var stream = new MemoryStream(binaryData))
            {
                var doc = XDocument.Load(stream);

                // use different logic to populate ViewModel based on XML format
                if (doc.Root.Attributes().Any(x => x.Value == Constants.PostSecondaryTranscripts.PescXmlNamespace))
                {
                    viewModel.LoadXml(doc, CultureInfo.GetCultureInfo(_locale), PostSecondaryTranscriptVersion.PESC);
                }
                else
                {
                    viewModel.LoadXml(doc, CultureInfo.GetCultureInfo(_locale), PostSecondaryTranscriptVersion.X12);
                }
            }

            // finally generate pdf from razor template and ViewModel
            var pdf = await _razorTemplateService.GeneratePostSecondaryTranscriptAsync(viewModel);

            return new BinaryDocument
            {
                Data = pdf,
                MimeType = Constants.MimeType.Pdf,
                CreatedDate = supportingDocumentBinaryData.CreatedOn,
                CreatedBy = "OCAS",
                Name = $"Transcript_{instituteName}.pdf".SanitizeFileName()
            };
        }

        private async Task<BinaryDocument> HandleStandardizedTest(GetSupportingDocumentFile request, Dto.Test standardizedTest)
        {
            if (standardizedTest.ApplicantId.IsEmpty())
            {
                throw new ValidationException($"No ApplicantId {standardizedTest.ApplicantId} on StandardizedTest {standardizedTest.Id}");
            }

            await _userAuthorization.CanAccessApplicantAsync(request.User, standardizedTest.ApplicantId);

            if (standardizedTest.IsOfficial != true)
            {
                throw new ValidationException($"Cannot generate pdf for unofficial StandardizedTest {standardizedTest.Id}");
            }

            var viewModel = _templateMapper.MapStandardizedTest(standardizedTest, await _lookupsCache.GetCountries(_locale), await _lookupsCache.GetProvinceStates(_locale), await _lookupsCache.GetCities(_locale), await _lookupsCache.GetStandardizedTestTypes(_locale));

            var translationsDictionary = await _translationsCache.GetTranslations(_locale);
            viewModel.LoadTranslations(translationsDictionary);

            // finally generate pdf from razor template and ViewModel
            var pdf = await _razorTemplateService.GenerateStandardizedTestAsync(viewModel);

            return new BinaryDocument
            {
                Data = pdf,
                MimeType = Constants.MimeType.Pdf,
                CreatedDate = standardizedTest.CreatedOn ?? DateTime.UtcNow,
                CreatedBy = "OCAS",
                Name = viewModel.TestType + ".pdf"
            };
        }

        private bool IsValidBase64(string base64String, out byte[] binaryData)
        {
            binaryData = null;
            try
            {
                if (!string.IsNullOrEmpty(base64String))
                {
                    binaryData = Convert.FromBase64String(base64String);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new ValidationException("Invalid Data");
            }
        }
    }
}