using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Data;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;
using ValidationException = Ocas.Common.Exceptions.ValidationException;

namespace Ocas.Domestic.Apply.Admin.Services.Handlers
{
    public class CreateOntarioStudentCourseCreditHandler : IRequestHandler<CreateOntarioStudentCourseCredit, OntarioStudentCourseCredit>
    {
        private readonly ILogger _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCache _lookupsCache;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDtoMapper _dtoMapper;
        private readonly IApiMapper _apiMapper;
        private readonly string _locale;

        public CreateOntarioStudentCourseCreditHandler(ILogger<CreateOntarioStudentCourseCreditHandler> logger, IDomesticContext domesticContext, ILookupsCache looksupsCache, IDtoMapper dtoMapper, IUserAuthorization userAuthorization, IApiMapper apiMapper, RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = looksupsCache ?? throw new ArgumentNullException(nameof(looksupsCache));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
        }

        public async Task<OntarioStudentCourseCredit> Handle(CreateOntarioStudentCourseCredit request, CancellationToken cancellationToken)
        {
            if (!_userAuthorization.IsOcasTier2User(request.User) && !_userAuthorization.IsHighSchoolUser(request.User))
            {
                throw new ForbiddenException();
            }

            // No duplicates allowed
            var ontarioStudentCourseCredits = await _domesticContext.GetOntarioStudentCourseCredits(new Dto.GetOntarioStudentCourseCreditOptions { ApplicantId = request.ApplicantId });
            if (ontarioStudentCourseCredits.Any(x => x.CourseCode == request.OntarioStudentCourseCredit.CourseCode && x.CompletedDate == request.OntarioStudentCourseCredit.CompletedDate))
            {
                throw new ValidationException($"OntarioStudentCourseCredit.CourseCode already exists for this Applicant: {request.OntarioStudentCourseCredit.CourseCode}");
            }

            return _apiMapper.MapOntarioStudentCourseCredit(await CreateGrade(request));
        }

        private async Task<Dto.OntarioStudentCourseCredit> CreateGrade(CreateOntarioStudentCourseCredit request)
        {
            var highSchools = await _lookupsCache.GetHighSchools(Constants.Localization.EnglishCanada);
            var supplierId = Guid.Empty;
            Dto.OntarioStudentCourseCredit ontarioStudentCourseCredit;
            Dto.Transcript transcript = null;

            var userType = _userAuthorization.GetUserType(request.User);

            switch (userType)
            {
                case UserType.HighSchoolUser:
                    {
                        var highSchool = highSchools.FirstOrDefault(x => x.Mident == request.User.GetPartnerId())
                            ?? throw new NotFoundException("Supplier mident not found");

                        supplierId = highSchool.Id;

                        var highSchoolTranscripts = await _domesticContext.GetTranscripts(
                            new Dto.GetTranscriptOptions { ContactId = request.ApplicantId, PartnerId = supplierId });

                        transcript = highSchoolTranscripts.FirstOrDefault(x => x.TranscriptType == TranscriptType.OntarioHighSchoolTranscript)
                              ?? throw new NotFoundException($"Ontario HS Transcript not found for applicant: {request.ApplicantId}");
                        break;
                    }

                case UserType.HighSchoolBoardUser:
                    {
                        var boardUserTranscripts = await _domesticContext.GetTranscripts(
                            new Dto.GetTranscriptOptions { ContactId = request.ApplicantId, BoardMident = request.User.GetPartnerId() });
                        if (boardUserTranscripts?.Any() != true) throw new NotFoundException("Supplier mident not found");

                        transcript = boardUserTranscripts.FirstOrDefault(x => x.TranscriptType == TranscriptType.OntarioHighSchoolTranscript);
                        if (transcript?.PartnerId == null) throw new NotFoundException($"Ontario HS Transcript not found for applicant: {request.ApplicantId}");

                        supplierId = transcript.PartnerId.Value;
                        break;
                    }

                case UserType.OcasUser:
                    {
                        var highSchool = highSchools.FirstOrDefault(x => x.Mident == request.OntarioStudentCourseCredit.SupplierMident)
                            ?? throw new NotFoundException("Supplier mident not found");
                        supplierId = highSchool.Id;

                        var ocasUserTranscripts = await _domesticContext.GetTranscripts(
                                            new Dto.GetTranscriptOptions { ContactId = request.ApplicantId, PartnerId = supplierId });

                        transcript = ocasUserTranscripts.FirstOrDefault(x => x.TranscriptType == TranscriptType.OntarioHighSchoolTranscript);
                        break;
                    }
            }

            if (supplierId.IsEmpty()) throw new NotFoundException("Supplier mident not found");

            await _domesticContext.BeginTransaction();
            try
            {
                var transcriptSources = await _lookupsCache.GetTranscriptSources(_locale);
                var transcriptSource = transcriptSources.FirstOrDefault(x => x.Code == Constants.TrancriptSources.OcasManual)
                    ?? throw new NotFoundException("Transcript source - OcasManual not found");

                // If OcasUser and no transcript found, Create one
                if (userType == UserType.OcasUser && transcript == null)
                {
                    var dtoTranscript = new Dto.TranscriptBase
                    {
                        ModifiedBy = request.User.GetUpnOrEmail(),
                        TranscriptType = TranscriptType.OntarioHighSchoolTranscript,
                        ContactId = request.ApplicantId,
                        PartnerId = supplierId,
                        TranscriptSourceId = transcriptSource.Id
                    };

                    transcript = await _domesticContext.CreateTranscript(dtoTranscript);
                }
                else
                {
                    transcript.TranscriptSourceId = transcriptSource.Id;
                    await _domesticContext.UpdateTranscript(transcript);
                }

                var ontarioStudentCourseCreditBase = new Dto.OntarioStudentCourseCreditBase();
                _dtoMapper.PatchOntarioStudentCourseCreditBase(ontarioStudentCourseCreditBase, request.OntarioStudentCourseCredit, request.User.GetUpnOrEmail(), transcript.Id);

                ontarioStudentCourseCredit = await _domesticContext.CreateOntarioStudentCourseCredit(ontarioStudentCourseCreditBase);

                await _domesticContext.CommitTransaction();
            }
            catch (Exception e)
            {
                await _domesticContext.RollbackTransaction(e);

                throw;
            }

            return ontarioStudentCourseCredit;
        }
    }
}