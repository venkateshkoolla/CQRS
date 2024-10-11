using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Data;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Validators.Models
{
    public class ProgramBaseValidator : AbstractValidator<ProgramBase>
    {
        private readonly ILookupsCache _lookupsCache;
        private readonly IDomesticContext _domesticContext;

        public ProgramBaseValidator(ILookupsCache lookupsCache, IDomesticContext domesticContext)
        {
            _lookupsCache = lookupsCache;
            _domesticContext = domesticContext;

            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(x => x.CollegeId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var colleges = await _lookupsCache.GetColleges(Constants.Localization.EnglishCanada);
                    return colleges.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.CampusId)
                .NotEmpty()
                .MustAsync(async (x, y, _) =>
                {
                    var campuses = await _lookupsCache.GetCampuses();
                    return campuses.Any(z => z.Id == y && z.CollegeId == x.CollegeId);
                })
                .WithMessage((x, y) => $"'{{PropertyName}}' {y} does not exist for CollegeId: {x.CollegeId}");

            RuleFor(x => x.Code)
                .NotEmpty()
                .Length(2, 8);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.DeliveryId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var programDeliveries = await _lookupsCache.GetProgramDeliveries(Constants.Localization.EnglishCanada);
                    return programDeliveries.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.ProgramTypeId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var programTypes = await _lookupsCache.GetProgramTypes(Constants.Localization.EnglishCanada);
                    return programTypes.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.Length)
                .InclusiveBetween(0, 100);

            RuleFor(x => x.LengthTypeId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var programLengthTypes = await _lookupsCache.GetProgramLengthTypes(Constants.Localization.EnglishCanada);
                    return programLengthTypes.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.CredentialId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var credentials = await _lookupsCache.GetCredentials(Constants.Localization.EnglishCanada);
                    return credentials.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.ApsNumber)
                .InclusiveBetween(0, 9999);

            RuleFor(x => x.EntryLevelIds)
                .NotEmpty()
                .MustAsync((x, y, _) => IsValidEntryLevels(x, y))
                .WithMessage("Please don't select entry level lower then default");

            RuleFor(x => x.DefaultEntryLevelId)
                .NotEmpty()
                .Must((x, y) => IsDefaultEntryLevelIncluded(x, y))
                .WithMessage("'{PropertyName}' should be included in list of entry levels");

            RuleFor(x => x.StudyAreaId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var studyAreas = await _lookupsCache.GetStudyAreas(Constants.Localization.EnglishCanada);
                    return studyAreas.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.HighlyCompetitiveId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var highlyCompetitives = await _lookupsCache.GetHighlyCompetitives(Constants.Localization.EnglishCanada);
                    return highlyCompetitives.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.LanguageId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var programLanguages = await _lookupsCache.GetProgramLanguages(Constants.Localization.EnglishCanada);
                    return programLanguages.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.LevelId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var levels = await _lookupsCache.GetProgramLevels(Constants.Localization.EnglishCanada);
                    return levels.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.McuCode)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var mcuCodes = await _lookupsCache.GetMcuCodes(Constants.Localization.EnglishCanada);
                    return mcuCodes.Any(z => z.Code == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.SpecialCode)
                .MustAsync(async (x, y, _) =>
                {
                    var specialCodes = await _domesticContext.GetProgramSpecialCodes(x.ApplicationCycleId);
                    return specialCodes.Any(z => z.Code == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}")
                .When(x => !string.IsNullOrWhiteSpace(x.SpecialCode));

            RuleFor(x => x.MinistryApprovalId)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var ministryApprovals = await _lookupsCache.GetMinistryApprovals(Constants.Localization.EnglishCanada);
                    return ministryApprovals.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.Url)
                .MaximumLength(1000)
                .Matches(@"(http(s)?:\/\/.)?(www\.)?[-a-zA-Z0-9@:%._\+~#=]{2,256}\.[a-z]{2,6}\b([-a-zA-Z0-9@:%_\+.~#?&//=]*)")
                .WithMessage("Please enter a valid website")
                .When(x => !string.IsNullOrEmpty(x.Url));

            RuleFor(x => x.ProgramCategory1Id)
                .NotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var programCategories = await _lookupsCache.GetProgramCategories(Constants.Localization.EnglishCanada);
                    return programCategories.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}");

            RuleFor(x => x.ProgramSubCategory1Id)
                .NotEmpty()
                .MustAsync(async (x, y, _) =>
                {
                    var programSubCategories = await _lookupsCache.GetProgramSubCategories(Constants.Localization.EnglishCanada);
                    return programSubCategories.Any(z => z.Id == y && z.CategoryId == x.ProgramCategory1Id);
                })
                .WithMessage((x, y) => $"'{{PropertyName}}' {y} does not exist for ProgramCategory1Id: {x.ProgramCategory1Id}");

            RuleFor(x => x.ProgramCategory2Id)
                .NullableGuidNotEmpty()
                .MustAsync(async (y, _) =>
                {
                    var programCategories = await _lookupsCache.GetProgramCategories(Constants.Localization.EnglishCanada);
                    return programCategories.Any(z => z.Id == y);
                })
                .WithMessage((_, y) => $"'{{PropertyName}}' does not exist: {y}")
                .When(x => x.ProgramCategory2Id.HasValue || x.ProgramSubCategory2Id.HasValue);

            RuleFor(x => x.ProgramSubCategory2Id)
                .NullableGuidNotEmpty()
                .MustAsync(async (x, y, _) =>
                {
                    var programSubCategories = await _lookupsCache.GetProgramSubCategories(Constants.Localization.EnglishCanada);
                    return programSubCategories.Any(z => z.Id == y && z.CategoryId == x.ProgramCategory2Id);
                })
                .WithMessage((x, y) => $"'{{PropertyName}}' {y} does not exist for ProgramCategory2Id: {x.ProgramCategory2Id}")
                .When(x => x.ProgramCategory2Id.HasValue || x.ProgramSubCategory2Id.HasValue);

            RuleFor(v => v.Intakes)
                .Must(v => !v.GroupBy(i => i.StartDate).Any(g => g.Skip(1).Any()))
                .WithMessage("Start dates must be unique for intakes (one per month)")
                .Must(v => !v.Skip(12).Any())
                .WithMessage("'{PropertyName}' cannot be more than 12 (one per month)")
                .MustAsync((v, w, _) => IsValidStartDate(v, w))
                .WithMessage("Start dates must be within the application cycle")
                .When(v => v.Intakes?.Any() == true);

            RuleForEach(x => x.Intakes)
                .SetValidator(y => new ProgramIntakeValidator(lookupsCache, y.DefaultEntryLevelId))
                .When(v => v.Intakes?.Any() == true);
        }

        private async Task<bool> IsValidEntryLevels(ProgramBase program, IEnumerable<Guid> entryLevels)
        {
            var entryLevelsLookups = await _lookupsCache.GetEntryLevels(Constants.Localization.EnglishCanada);
            var codeForDefaultEntryLevelId = entryLevelsLookups.FirstOrDefault(e => e.Id == program.DefaultEntryLevelId)?.Code;
            var fullEntryLevels = entryLevelsLookups.Where(p => entryLevels.Any(p2 => p2 == p.Id));
            return fullEntryLevels.All(p => string.Compare(codeForDefaultEntryLevelId, p.Code, StringComparison.InvariantCultureIgnoreCase) <= 0);
        }

        private bool IsDefaultEntryLevelIncluded(ProgramBase program, Guid? defaultEntryLevel)
        {
            return program.EntryLevelIds.Contains(defaultEntryLevel ?? Guid.Empty);
        }

        private async Task<bool> IsValidStartDate(ProgramBase programBase, IEnumerable<ProgramIntake> intakes)
        {
            var collegeApplicationCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var crmAppCycle = collegeApplicationCycles.First(x => x.Id == programBase.ApplicationCycleId);
            if (crmAppCycle == null) return false;

            var appCycleYear = int.Parse(crmAppCycle.Year);

            var validStartDates = new List<string>();
            for (var i = 8; i <= 19; i++)
            {
                var year = i > 12 ? appCycleYear + 1 : appCycleYear;
                var month = i > 12 ? i - 12 : i;
                var paddedMonth = month.ToString("#00");
                validStartDates.Add(year.ToString().Substring(2) + paddedMonth);
            }

            return intakes.All(i => validStartDates.Contains(i.StartDate));
        }
    }
}
