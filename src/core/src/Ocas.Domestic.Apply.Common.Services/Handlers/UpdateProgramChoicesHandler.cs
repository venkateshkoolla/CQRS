using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Common.Exceptions;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Services.Extensions;
using Ocas.Domestic.Apply.Services.Mappers;
using Ocas.Domestic.Apply.Services.Messages;
using Ocas.Domestic.Data;
using Ocas.Domestic.Data.Extras;
using Dto = Ocas.Domestic.Models;
using DtoEnum = Ocas.Domestic.Enums;

namespace Ocas.Domestic.Apply.Services.Handlers
{
    public class UpdateProgramChoicesHandler : IRequestHandler<UpdateProgramChoices, IList<ProgramChoice>>
    {
        private readonly ILogger<UpdateProgramChoicesHandler> _logger;
        private readonly IUserAuthorizationBase _userAuthorization;
        private readonly IDomesticContext _domesticContext;
        private readonly IDomesticContextExtras _domesticContextExtras;
        private readonly string _locale;
        private readonly ILookupsCacheBase _lookupsCache;
        private readonly IApiMapperBase _apiMapper;
        private readonly IDtoMapperBase _dtoMapper;
        private readonly string _sourcePartner;

        public UpdateProgramChoicesHandler(
            ILogger<UpdateProgramChoicesHandler> logger,
            IUserAuthorizationBase userAuthorization,
            IDomesticContext domesticContext,
            ILookupsCacheBase lookupsCache,
            IApiMapperBase apiMapper,
            IDtoMapperBase dtoMapperBase,
            IDomesticContextExtras domesticContextExtras,
            RequestCache requestCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _userAuthorization = userAuthorization ?? throw new ArgumentNullException(nameof(userAuthorization));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _dtoMapper = dtoMapperBase ?? throw new ArgumentNullException(nameof(dtoMapperBase));
            _domesticContextExtras = domesticContextExtras ?? throw new ArgumentNullException(nameof(domesticContextExtras));

            if (requestCache == null) throw new ArgumentNullException(nameof(requestCache));
            _locale = requestCache.Get<CultureInfo>()?.Name;
            _sourcePartner = requestCache.Get<string>(Constants.RequestCacheKeys.Partner);
        }

        public async Task<IList<ProgramChoice>> Handle(UpdateProgramChoices request, CancellationToken cancellationToken)
        {
            var application = await _domesticContext.GetApplication(request.ApplicationId)
                ?? throw new NotFoundException("Application Id not found");
            await _userAuthorization.CanAccessApplicantAsync(request.User, application.ApplicantId);
            await ValidateApplication(application);

            await ValidateApplicant(application.ApplicantId, request.ProgramChoices.ToList());

            var intakeIds = request.ProgramChoices.Select(c => c.IntakeId).Distinct().ToList();
            IList<Dto.ProgramIntake> intakes = new List<Dto.ProgramIntake>();
            if (intakeIds.Any())
            {
                intakes = await _domesticContext.GetProgramIntakes(new Dto.GetProgramIntakeOptions { Ids = intakeIds, StateCode = null, StatusCode = null });
            }

            var dtoExistingChoices = await _domesticContext.GetProgramChoices(new Dto.GetProgramChoicesOptions { ApplicationId = request.ApplicationId });
            var programs = await ValidateIntakes(application, intakes, request.ProgramChoices, dtoExistingChoices.ToList());

            await ValidateChoices(request.ProgramChoices.ToList(), dtoExistingChoices.ToList());

            (var hasWritten, var exc) = await UpdateProgramChoices(request, application, intakes.ToList(), programs.ToList(), dtoExistingChoices.ToList());
            if (exc != null)
            {
                if (hasWritten)
                    _logger.LogCritical(exc, "Program choices has failed to fully update", request.ProgramChoices);

                throw exc;
            }

            var dtoUpdatedChoices = await _domesticContext.GetProgramChoices(new Dto.GetProgramChoicesOptions { ApplicationId = request.ApplicationId });
            var orderedUpdateChoices = dtoUpdatedChoices
                .Where(c => c.SequenceNumber <= Constants.ProgramChoices.MaxTotalChoices)
                .OrderBy(c => c.SequenceNumber).ToList();

            IList<Dto.ShoppingCartDetail> shoppingCartDetails = null;
            if (orderedUpdateChoices.Any(x => x.SupplementalFeePaid == false))
            {
                shoppingCartDetails = await _domesticContext.GetShoppingCartDetails(
                    new Dto.GetShoppingCartDetailOptions
                    {
                        ApplicationId = request.ApplicationId
                    },
                    _locale.ToLocaleEnum());
            }

            return _apiMapper.MapProgramChoices(orderedUpdateChoices, intakes, await _lookupsCache.GetProgramIntakeAvailabilities(_locale), shoppingCartDetails);
        }

        private static IEnumerable<Dto.ProgramChoice> GetChoicesToRemove(List<ProgramChoiceBase> choices, List<Dto.ProgramChoice> existingChoices)
        {
            // Choices being removed that are not alternates
            return existingChoices.Where(c => !choices.Any(i => i.IntakeId == c.ProgramIntakeId && i.EntryLevelId == c.EntryLevelId)
                                                                         && c.SequenceNumber <= Constants.ProgramChoices.MaxTotalChoices);
        }

        private async Task<(bool hasWritten, Exception e)> UpdateProgramChoices(UpdateProgramChoices request, Dto.Application application, List<Dto.ProgramIntake> intakes, List<Dto.Program> programs, List<Dto.ProgramChoice> dtoExistingChoices)
        {
            var anyModifications = false;
            try
            {
                var modifiedBy = request.User.GetUpnOrEmail();

                (var hasRemoved, var removeExc) = await RemoveChoices(request.ProgramChoices, dtoExistingChoices, modifiedBy);
                anyModifications |= hasRemoved;
                if (removeExc != null) throw removeExc;

                (var hasUpserted, var upsertExc) = await UpsertChoices(request.ProgramChoices, dtoExistingChoices, intakes, programs, application, modifiedBy);
                anyModifications |= hasUpserted;
                if (upsertExc != null) throw upsertExc;

                (var hasAppUpdated, var appUpdateExc) = await UpdateApplication(application, modifiedBy);
                anyModifications |= hasAppUpdated;
                if (appUpdateExc != null) throw appUpdateExc;
            }
            catch (Exception exc)
            {
                return (anyModifications, new ConflictException("Program choices update incomplete. Please retry.", exc));
            }

            return (anyModifications, null);
        }

        private async Task<(bool hasWritten, Exception e)> UpsertChoices(List<ProgramChoiceBase> choices, List<Dto.ProgramChoice> existingChoices, List<Dto.ProgramIntake> programIntakes, List<Dto.Program> programs, Dto.Application application, string modifiedBy)
        {
            var hasWritten = false;
            var applicationStatuses = await _lookupsCache.GetApplicationStatuses(Constants.Localization.EnglishCanada);
            var applicationStatus = applicationStatuses.FirstOrDefault(s => s.Id == application.ApplicationStatusId) ??
                throw new UnhandledException($"Application Status not found:{application.ApplicationStatusId}");

            var effectiveDate = applicationStatus.Code == Constants.ApplicationStatuses.Active ? DateTime.UtcNow.ToDateInEstAsUtc() : (DateTime?)null;
            var sourceId = await _lookupsCache.GetSourceId(_sourcePartner);

            for (var i = 0; i < choices.Count; i++)
            {
                var newChoice = choices[i];
                newChoice.ApplicantId = application.ApplicantId;
                var sequenceNumber = i + 1;
                var intake = programIntakes.First(pi => pi.Id == newChoice.IntakeId);
                var program = programs.First(p => intake.ProgramId == p.Id);
                var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
                var college = colleges.First(c => c.Id == program.CollegeId);

                try
                {
                    var duplicateChoice = existingChoices.SingleOrDefault(c => c.ProgramIntakeId == newChoice.IntakeId && c.EntryLevelId == newChoice.EntryLevelId);

                    if (duplicateChoice is null)
                    {
                        var dbDto = new Dto.ProgramChoiceBase
                        {
                            ModifiedBy = modifiedBy,
                            SequenceNumber = sequenceNumber,
                            EffectiveDate = effectiveDate,
                            SourceId = sourceId,
                            Name = $"{application.ApplicationNumber}-{college.Code}-{program.Code}"
                        };
                        _dtoMapper.PatchProgramChoice(dbDto, newChoice);
                        await _domesticContext.CreateProgramChoice(dbDto);
                        hasWritten = true;
                    }
                    else
                    {
                        duplicateChoice.ModifiedBy = modifiedBy;
                        duplicateChoice.SequenceNumber = sequenceNumber;
                        duplicateChoice.EffectiveDate = effectiveDate;
                        duplicateChoice.SourceId = duplicateChoice.SourceId.IsEmpty() ? sourceId : duplicateChoice.SourceId;
                        duplicateChoice.Name = string.IsNullOrEmpty(duplicateChoice.Name) ? $"{application.ApplicationNumber}-{college.Code}-{program.Code}" : duplicateChoice.Name;
                        _dtoMapper.PatchProgramChoice(duplicateChoice, newChoice);
                        await _domesticContext.UpdateProgramChoice(duplicateChoice);
                        hasWritten = true;
                    }
                }
                catch (Exception e)
                {
                    return (hasWritten, e); // short circuit early, no point to continue
                }
            }

            return (hasWritten, null);
        }

        private async Task<(bool hasWritten, Exception e)> RemoveChoices(List<ProgramChoiceBase> choices, List<Dto.ProgramChoice> existingChoices, string modifiedBy)
        {
            var hasWritten = false;
            foreach (var choice in GetChoicesToRemove(choices, existingChoices))
            {
                try
                {
                    choice.ModifiedBy = modifiedBy;
                    await _domesticContext.UpdateProgramChoice(choice);
                    await _domesticContext.DeleteProgramChoice(choice);
                    hasWritten = true;
                }
                catch (Exception e)
                {
                    return (hasWritten, e); // short circuit early, no point to continue
                }
            }

            return (hasWritten, null);
        }

        private async Task<(bool hasWritten, Exception e)> UpdateApplication(Dto.Application application, string modifiedBy)
        {
            var hasWritten = false;
            try
            {
                var applicant = await _domesticContext.GetContact(application.ApplicantId);
                var hasChanges = await _domesticContextExtras.PatchBasisForAdmission(
                    application,
                    applicant,
                    modifiedBy,
                    await _lookupsCache.GetBasisForAdmissionsDto(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetCurrentsDto(Constants.Localization.EnglishCanada),
                    await _lookupsCache.GetApplicationCyclesDto());

                if (hasChanges)
                {
                    await _domesticContext.UpdateApplication(application);
                    hasWritten = true;
                }
            }
            catch (Exception e)
            {
                return (hasWritten, e); // short circuit early, no point to continue
            }

            return (hasWritten, null);
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

        private async Task ValidateApplicant(Guid applicantId, List<ProgramChoiceBase> choices)
        {
            var choicesWithCollegeHistory = choices.Where(c => c.PreviousYearApplied != null || c.PreviousYearAttended != null);

            if (choicesWithCollegeHistory.Any())
            {
                var applicant = await _domesticContext.GetContact(applicantId);
                if (choicesWithCollegeHistory.Any(c => c.PreviousYearApplied < applicant.BirthDate.Year || c.PreviousYearAttended < applicant.BirthDate.Year))
                    throw new ValidationException($"College previous year(s) cannot be before applicant year of birth: {applicant.BirthDate.Year}");

                if (choicesWithCollegeHistory.Any(c => c.PreviousYearApplied > DateTime.UtcNow.ToDateInEstAsUtc().Year || c.PreviousYearAttended > DateTime.UtcNow.ToDateInEstAsUtc().Year))
                    throw new ValidationException("College previous year(s) cannot be in the future.");
            }
        }

        private async Task<IList<Dto.Program>> ValidateIntakes(Dto.Application application, IList<Dto.ProgramIntake> intakes, IList<ProgramChoiceBase> programChoices, List<Dto.ProgramChoice> existingChoices)
        {
            var intakeAvailabilities = await _lookupsCache.GetProgramIntakeAvailabilities(Constants.Localization.EnglishCanada);
            var intakeAvailabiltyClosedId = intakeAvailabilities.First(s => s.Code == Constants.ProgramIntakeAvailabilities.Closed).Id;
            var activeIntakes = intakes.Where(i => i.State == DtoEnum.State.Active
                                                && i.Status == DtoEnum.Status.Active
                                                && i.AvailabilityId != intakeAvailabiltyClosedId);

            var promotions = await _lookupsCache.GetPromotions(Constants.Localization.EnglishCanada);
            var promotionId = promotions.FirstOrDefault(p => p.Code == Constants.Promotions.Promotional)?.Id ??
                throw new UnhandledException("Promotion cannot be be found.");
            if (activeIntakes.Any(i => i.PromotionId == promotionId))
                throw new ValidationException("Intakes cannot be promotional.");

            var intakeStatuses = await _lookupsCache.GetProgramIntakeStatuses(Constants.Localization.EnglishCanada);
            var intakeStatusActiveId = intakeStatuses.First(s => s.Code == Constants.ProgramIntakeStatuses.Active).Id;
            if (activeIntakes.Any(i => i.ProgramIntakeStatusId != intakeStatusActiveId))
                throw new ValidationException("Intakes must be active.");

            //Validate each intake against it's program
            var programs = new List<Dto.Program>();
            var choicesByCollege = new List<KeyValuePair<Guid, ProgramChoiceBase>>(programChoices.Count);
            foreach (var intake in intakes)
            {
                var program = await _domesticContext.GetProgram(intake.ProgramId);
                programs.Add(program);

                var colleges = await _lookupsCache.GetColleges(_locale);
                var college = colleges.FirstOrDefault(c => c.Code == _sourcePartner && c.AllowCba && !c.AllowCbaMultiCollegeApply);
                if (college != null && college.Id != program.CollegeId)
                {
                    throw new ValidationException($"Program college must match {_sourcePartner}.");
                }

                var collegeCycles = await _lookupsCache.GetCollegeApplicationCycles();
                var collegeCycle = collegeCycles.FirstOrDefault(c => c.MasterId == application.ApplicationCycleId && c.CollegeId == program.CollegeId) ??
                    throw new ValidationException("College application cycle not found.");

                if (intake.CollegeApplicationCycleId != collegeCycle.Id)
                    throw new ValidationException("Intake must be in the application's cycle.");

                if (!activeIntakes.Any(i => programChoices.Any(c => c.IntakeId == i.Id) && i.AvailabilityId == intakeAvailabiltyClosedId))
                {
                    var existingClosedIntakeChoices = existingChoices.Where(c => !activeIntakes.Any(i => c.ProgramIntakeId == i.Id));
                    var closedIntakeChoices = programChoices.Where(c => !activeIntakes.Any(i => c.IntakeId == i.Id));
                    if (existingClosedIntakeChoices?.Any(c => !closedIntakeChoices.Any(e => e.EntryLevelId == c.EntryLevelId)) != false)
                        throw new ValidationException("Choice for a closed intake cannot update entry level id.");

                    if (closedIntakeChoices?.Any(c => !existingClosedIntakeChoices.Any(e => e.EntryLevelId == c.EntryLevelId)) != false)
                        throw new ValidationException("Intakes must be open or waitlisted.");
                }

                foreach (var programChoice in programChoices.Where(c => c.IntakeId == intake.Id))
                {
                    if (!existingChoices.Any(c => c.ProgramIntakeId == programChoice.IntakeId && c.EntryLevelId == programChoice.EntryLevelId))
                    {
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
                    }

                    choicesByCollege.Add(new KeyValuePair<Guid, ProgramChoiceBase>(program.CollegeId, programChoice));
                    if (choicesByCollege.GroupBy(c => c.Key, (key, values) => new { Id = key, Count = values.Count() }).Any(g => g.Count > Constants.ProgramChoices.MaxCollegeChoices))
                        throw new ValidationException($"No more than {Constants.ProgramChoices.MaxCollegeChoices} choices per college.");
                }

                // Collate college previous years to the max set
                var collegeMaxHistory = choicesByCollege
                        .GroupBy(c => c.Key)
                        .Select(g => new
                        {
                            CollegeId = g.Key,
                            PreviousYearApplied = g.Max(r => r.Value.PreviousYearApplied),
                            PreviousYearAttended = g.Max(r => r.Value.PreviousYearAttended)
                        }).First(c => c.CollegeId == program.CollegeId);

                foreach (var programChoice in programChoices.Where(c => c.IntakeId == intake.Id))
                {
                    programChoice.PreviousYearApplied = collegeMaxHistory.PreviousYearApplied;
                    programChoice.PreviousYearAttended = collegeMaxHistory.PreviousYearAttended;
                }
            }

            return programs;
        }

        private async Task ValidateChoices(List<ProgramChoiceBase> choices, List<Dto.ProgramChoice> existingChoices)
        {
            var offerStatuses = await _lookupsCache.GetOfferStatuses(Constants.Localization.EnglishCanada);
            var offerStatusAcceptedId = offerStatuses.First(s => s.Code == Constants.Offers.Status.Accepted).Id;

            var choicesToRemove = GetChoicesToRemove(choices, existingChoices);
            if (choicesToRemove.Any(c => c.OfferStatusId == offerStatusAcceptedId))
            {
                throw new ValidationException($"Intake offer must not be accepted for intake id(s): {string.Join(", ", choicesToRemove.Select(o => o.ProgramIntakeId))}");
            }

            var alternateChoicesToDuplicate = existingChoices.Where(c => choices.Any(i => i.IntakeId == c.ProgramIntakeId && i.EntryLevelId == c.EntryLevelId && c.SequenceNumber > Constants.ProgramChoices.MaxTotalChoices));
            if (alternateChoicesToDuplicate.Any())
            {
                throw new ValidationException($"Alternate choice(s) already exists for: {string.Join(", ", alternateChoicesToDuplicate.Select(o => $"IntakeId: {o.ProgramIntakeId} with EntryLevel: {o.EntryLevelId}"))}");
            }
        }
    }
}
