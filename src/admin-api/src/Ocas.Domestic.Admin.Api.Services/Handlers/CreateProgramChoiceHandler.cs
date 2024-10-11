using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Admin.Api.Services.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class CreateProgramChoiceHandler : IRequestHandler<CreateProgramChoice, ApplicationSummary>
    {
        private readonly ILogger<CreateProgramChoiceHandler> _logger;
        private readonly IUserAuthorization _userAuthorization;
        private readonly IDomesticContext _domesticContext;
        private readonly IAppSettingsExtras _appSettingsExtras;
        private readonly string _locale;
        private readonly ILookupsCache _lookupsCache;
        private readonly IApiMapper _apiMapper;
        private readonly IDtoMapper _dtoMapper;

        public CreateProgramChoiceHandler(
            ILogger<CreateProgramChoiceHandler> logger,
            IUserAuthorization userAuthorization,
            IDomesticContext domesticContext,
            ILookupsCache lookupsCache,
            IApiMapper apiMapper,
            IDtoMapper dtoMapper,
            IAppSettingsExtras appSettingsExtras,
            RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _dtoMapper = dtoMapper ?? throw new ArgumentNullException(nameof(dtoMapper));
            _appSettingsExtras = appSettingsExtras ?? throw new ArgumentNullException(nameof(appSettingsExtras));

            if (requestCache == null) throw new ArgumentNullException(nameof(requestCache));
            _locale = requestCache.Get<CultureInfo>()?.Name;
        }

        public async Task<ApplicationSummary> Handle(CreateProgramChoice request, CancellationToken cancellationToken)
        {
            if (!_userAuthorization.IsOcasUser(request.User)) throw new NotAuthorizedException();

            var application = await _domesticContext.GetApplication(request.ApplicationId)
                ?? throw new NotFoundException("Application Id not found");
            await ValidateApplication(application);

            var program = await _domesticContext.GetProgram(request.ProgramChoice.ProgramId);

            var existingChoices = await _domesticContext.GetProgramChoices(new Dto.GetProgramChoicesOptions { ApplicationId = application.Id });
            if (existingChoices.Any()) ValidateChoice(request.ProgramChoice, existingChoices.ToList(), program.CollegeId);

            var intakes = await _domesticContext.GetProgramIntakes(new Dto.GetProgramIntakeOptions { FromDate = request.ProgramChoice.StartDate, ProgramId = program.Id });
            var intake = await ValidateIntakes(application, program, intakes.ToList(), request.ProgramChoice);

            await CreateProgramChoice(request, application, program, intake.Id, existingChoices.ToList());

            var dtoApplicantSummary = await _domesticContext.GetApplicantSummary(new Dto.GetApplicantSummaryOptions
            {
                ApplicationId = application.Id,
                ApplicantId = application.ApplicantId,
                Locale = _locale.ToLocaleEnum()
            });
            var dtoApplicationSummary = dtoApplicantSummary.ApplicationSummaries.FirstOrDefault(s => s.Application.Id == application.Id) ??
                   throw new NotFoundException("Application summary not found.");

            return _apiMapper.MapApplicationSummary(
                        dtoApplicationSummary,
                        dtoApplicantSummary.Contact,
                        await GetProgramIntakes(dtoApplicationSummary.ProgramChoices),
                        await _lookupsCache.GetApplicationStatuses(_locale),
                        await _lookupsCache.GetInstituteTypes(_locale),
                        await _lookupsCache.GetTranscriptTransmissions(_locale),
                        await _lookupsCache.GetOfferStates(_locale),
                        await _lookupsCache.GetProgramIntakeAvailabilities(_locale),
                        _appSettingsExtras);
        }

        private async Task CreateProgramChoice(CreateProgramChoice request, Dto.Application application, Dto.Program program, Guid intakeId, List<Dto.ProgramChoice> existingChoices)
        {
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            var applicationStatus = applicationStatuses.FirstOrDefault(s => s.Id == application.ApplicationStatusId) ??
                throw new ValidationException($"Application Status not found:{application.ApplicationStatusId}");

            var effectiveDate = applicationStatus.Code == Constants.ApplicationStatuses.Active ? DateTime.UtcNow.ToDateInEstAsUtc() : (DateTime?)null;

            var colleges = await _lookupsCache.GetColleges(_locale);
            var college = colleges.FirstOrDefault(c => c.Id == program.CollegeId) ??
                throw new NotFoundException($"College not found : {program.CollegeId}");

            var choice = new ProgramChoiceBase
            {
                ApplicantId = application.ApplicantId,
                ApplicationId = request.ProgramChoice.ApplicationId,
                EffectiveDate = effectiveDate.ToStringOrDefault(),
                EntryLevelId = request.ProgramChoice.EntryLevelId,
                IntakeId = intakeId,
                PreviousYearApplied = null,
                PreviousYearAttended = null
            };

            if (existingChoices.Any(e => e.CollegeId == college.Id)) CollateCollegePreviousYear(choice, existingChoices, college.Id);

            var sources = await _lookupsCache.GetSources(_locale);
            var source = sources.FirstOrDefault(x => x.Code == Constants.Sources.A2C2) ??
                throw new NotFoundException($"Source {Constants.Sources.A2C2} not found.");

            var sequenceNumber = existingChoices.Count + 1;
            var dbDto = new Dto.ProgramChoiceBase
            {
                ModifiedBy = request.User.GetUpnOrEmail(),
                SequenceNumber = sequenceNumber,
                EffectiveDate = effectiveDate,
                SourceId = source.Id,
                Name = $"{application.ApplicationNumber}-{college.Code}-{program.Code}"
            };
            _dtoMapper.PatchProgramChoice(dbDto, choice);
            await _domesticContext.CreateProgramChoice(dbDto);
        }

        private async Task<IList<Dto.ProgramIntake>> GetProgramIntakes(IList<Dto.ProgramChoice> programChoices)
        {
            var filterChoices = programChoices.Where(c => c.SequenceNumber <= Constants.ProgramChoices.MaxTotalChoices).ToList();
            IList<Dto.ProgramIntake> intakes = null;
            if (filterChoices.Any())
            {
                var intakeIds = filterChoices.Select(c => c.ProgramIntakeId).Distinct().ToList();
                intakes = await _domesticContext.GetProgramIntakes(new Dto.GetProgramIntakeOptions { Ids = intakeIds });
            }

            return intakes;
        }

        private async Task ValidateApplication(Dto.Application application)
        {
            var applicationCycles = await _lookupsCache.GetApplicationCycles();
            var applicationCycle = applicationCycles.FirstOrDefault(a => a.Id == application.ApplicationCycleId) ??
                throw new ValidationException("Application cycle not found.");

            var applicationCycleStatuses = await _lookupsCache.GetApplicationCycleStatuses(Constants.Localization.EnglishCanada);
            var applicationCycleStatus = applicationCycleStatuses.FirstOrDefault(s => s.Code == applicationCycle.Status);

            if (applicationCycleStatus.Code != Constants.ApplicationCycleStatuses.Active)
                throw new ValidationException("Application cycle must be active.");
        }

        private async Task<Dto.ProgramIntake> ValidateIntakes(Dto.Application application, Dto.Program program, List<Dto.ProgramIntake> intakes, CreateProgramChoiceRequest programChoice)
        {
            var intake = intakes.SingleOrDefault();

            // TODO When overriding create the intake
            if (intake == null)
                throw new ValidationException("Intake with requested start date does not exist.");

            var collegeCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var collegeCycle = collegeCycles.FirstOrDefault(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == program.CollegeId) ??
                throw new ValidationException("College application cycle not found.");

            if (intake.CollegeApplicationCycleId != collegeCycle.Id)
                throw new ValidationException("Intake must be in the application's cycle.");

            if (!intake.EntryLevels.Contains(programChoice.EntryLevelId))
                throw new ValidationException("Program choice's entry level is not on intake.");

            var entryLevelsLookup = await _lookupsCache.GetEntryLevels(Constants.Localization.EnglishCanada);

            var selectedEntrylevelIndex = entryLevelsLookup.FindIndex(x => x.Id == programChoice.EntryLevelId);
            var expectedMinEntryLevelIndex = entryLevelsLookup.FindIndex(x => x.Id == intake.DefaultEntrySemesterId);

            if (selectedEntrylevelIndex < expectedMinEntryLevelIndex)
            {
                _logger.LogCritical($"Program choice entry level selected  {programChoice.EntryLevelId} is below the default level  {intake.DefaultEntrySemesterId} .");
                throw new ValidationException($"Program choice entry level selected  {programChoice.EntryLevelId} is below the default level  {intake.DefaultEntrySemesterId} .");
            }

            return intake;
        }

        private void CollateCollegePreviousYear(ProgramChoiceBase programChoice, List<Dto.ProgramChoice> existingChoices, Guid collegeId)
        {
            var choicesByCollege = new List<KeyValuePair<Guid, ProgramChoiceBase>>();

            foreach (var dto in existingChoices)
            {
                var existingChoice = new ProgramChoiceBase
                {
                    ApplicantId = dto.ApplicantId,
                    ApplicationId = dto.ApplicationId,
                    EffectiveDate = dto.EffectiveDate.ToStringOrDefault(),
                    EntryLevelId = dto.EntryLevelId,
                    IntakeId = dto.ProgramIntakeId,
                    PreviousYearApplied = dto.PreviousYearApplied,
                    PreviousYearAttended = dto.PreviousYearAttended
                };
                choicesByCollege.Add(new KeyValuePair<Guid, ProgramChoiceBase>(dto.CollegeId.Value, existingChoice));
            }

            // Collate college previous years to the max set
            var collegeMaxHistory = choicesByCollege
                    .GroupBy(c => c.Key)
                    .Select(g => new
                    {
                        CollegeId = g.Key,
                        PreviousYearApplied = g.Max(r => r.Value.PreviousYearApplied),
                        PreviousYearAttended = g.Max(r => r.Value.PreviousYearAttended)
                    }).First(c => c.CollegeId == collegeId);

            programChoice.PreviousYearApplied = collegeMaxHistory.PreviousYearApplied;
            programChoice.PreviousYearAttended = collegeMaxHistory.PreviousYearAttended;
        }

        private void ValidateChoice(CreateProgramChoiceRequest choice, List<Dto.ProgramChoice> existingChoices, Guid collegeId)
        {
            if (existingChoices.Count >= Constants.ProgramChoices.MaxTotalChoices)
                throw new ValidationException($"No more than {Constants.ProgramChoices.MaxTotalChoices} choices.");

            if (existingChoices.Count(e => e.CollegeId == collegeId) >= Constants.ProgramChoices.MaxCollegeChoices)
                throw new ValidationException($"No more than {Constants.ProgramChoices.MaxCollegeChoices} choices for college: {collegeId}");

            var existingChoice = existingChoices.FirstOrDefault(c => c.IntakeStartDate == choice.StartDate && c.EntryLevelId == choice.EntryLevelId);

            if (existingChoice != null)
            {
                throw new ValidationException(
                    existingChoice.SequenceNumber > Constants.ProgramChoices.MaxTotalChoices
                    ? "Alternate program choice with intake and entry level already exists."
                    : "Program choice with intake and entry level already exists.");
            }
        }
    }
}
