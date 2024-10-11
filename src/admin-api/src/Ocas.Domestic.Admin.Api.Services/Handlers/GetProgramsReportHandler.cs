using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClosedXML.Excel;
using MediatR;
using Microsoft.Extensions.Logging;
using Ocas.Domestic.Apply.Admin.Api.Services.Extensions;
using Ocas.Domestic.Apply.Admin.Api.Services.Mappers;
using Ocas.Domestic.Apply.Admin.Enums;
using Ocas.Domestic.Apply.Admin.Messages;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Core.Extensions;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.Data;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Handlers
{
    public class GetProgramsReportHandler : IRequestHandler<GetProgramsReport, BinaryDocument>
    {
        private readonly ILogger<GetProgramsReportHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;
        private readonly IApiMapper _apiMapper;
        private readonly ITranslationsCache _translationsCache;

        public GetProgramsReportHandler(
            ILogger<GetProgramsReportHandler> logger,
            IDomesticContext domesticContext,
            ILookupsCache lookupsCache,
            RequestCache requestCache,
            IApiMapper apiMapper,
            ITranslationsCache translationsCache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _domesticContext = domesticContext ?? throw new ArgumentNullException(nameof(domesticContext));
            _lookupsCache = lookupsCache ?? throw new ArgumentNullException(nameof(lookupsCache));
            _locale = requestCache.Get<CultureInfo>()?.Name ?? throw new ArgumentNullException(nameof(requestCache));
            _apiMapper = apiMapper ?? throw new ArgumentNullException(nameof(apiMapper));
            _translationsCache = translationsCache ?? throw new ArgumentNullException(nameof(translationsCache));
        }

        public async Task<BinaryDocument> Handle(GetProgramsReport request, CancellationToken cancellationToken)
        {
            var spreadsheetArray = Array.Empty<byte>();
            var dtoPrograms = await GetPrograms(request);
            var collegeApplicationCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var collegeApplicationCycle = collegeApplicationCycles.FirstOrDefault(c => c.CollegeId == request.CollegeId && c.MasterId == request.ApplicationCycleId);

            if (dtoPrograms.Any())
            {
                var allEntryLevels = await _lookupsCache.GetEntryLevels(_locale);
                var exportPrograms = await GetProgramExports(dtoPrograms, collegeApplicationCycle, allEntryLevels, request.Params);
                spreadsheetArray = await GetSpreadSheetBytes(allEntryLevels, exportPrograms, dtoPrograms);
            }

            var fileName = await _translationsCache.GetTranslationValue(_locale, "report.program.workbook", Constants.Translations.ApplyAdminApi);
            return new BinaryDocument
            {
                Name = $"{fileName} {collegeApplicationCycle.Name}.xlsx",
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                CreatedDate = DateTime.Now,
                CreatedBy = request.User.GetDisplayName(),
                Data = spreadsheetArray
            };
        }

        private Task<IList<Dto.Program>> GetPrograms(GetProgramsReport request)
        {
            var options = new Dto.GetProgramsOptions
            {
                ApplicationCycleId = request.ApplicationCycleId,
                CollegeId = request.CollegeId,
                CampusId = request.Params.CampusId,
                DeliveryId = request.Params.DeliveryId,
                Code = request.Params.Code,
                Title = request.Params.Title
            };

            return _domesticContext.GetPrograms(options);
        }

        private async Task<IList<ProgramExport>> GetProgramExports(IList<Dto.Program> dtoPrograms, CollegeApplicationCycle collegeApplicationCycle, IList<LookupItem> allEntryLevels, GetProgramOptions options)
        {
            var colleges = await _lookupsCache.GetColleges(_locale);
            var campuses = await _lookupsCache.GetCampuses();
            var studyMethods = await _lookupsCache.GetStudyMethods(_locale);
            var mcuCodes = await _lookupsCache.GetMcuCodes(_locale);
            var programTypes = await _lookupsCache.GetProgramTypes(_locale);
            var promotions = await _lookupsCache.GetPromotions(_locale);
            var programLengths = await _lookupsCache.GetProgramLengths(_locale);
            var adultTrainings = await _lookupsCache.GetAdultTrainings(_locale);
            var credentials = await _lookupsCache.GetCredentials(_locale);
            var studyAreas = await _lookupsCache.GetStudyAreas(_locale);
            var highlyCompetitives = await _lookupsCache.GetHighlyCompetitives(_locale);
            var programLanguages = await _lookupsCache.GetProgramLanguages(_locale);
            var ministryApprovals = await _lookupsCache.GetMinistryApprovals(_locale);
            var programCategories = await _lookupsCache.GetProgramCategories(_locale);
            var programSubCategories = await _lookupsCache.GetProgramSubCategories(_locale);
            var specialCodes = _apiMapper.MapSpecialCodes(await _domesticContext.GetProgramSpecialCodes(collegeApplicationCycle.Id));

            var programExports =
                _apiMapper.MapProgramExports(dtoPrograms, collegeApplicationCycle, colleges, campuses, studyMethods, mcuCodes, specialCodes, programTypes, promotions, programLengths, adultTrainings, credentials, studyAreas, highlyCompetitives, programLanguages, allEntryLevels, ministryApprovals, programCategories, programSubCategories);
            switch (options.SortBy)
            {
                case ProgramSortField.Code:
                    return options.SortDirection == SortDirection.Ascending
                        ? programExports.OrderBy(x => x.ProgramCode).ToList()
                        : programExports.OrderByDescending(x => x.ProgramCode).ToList();
                case ProgramSortField.Title:
                    return options.SortDirection == SortDirection.Ascending
                        ? programExports.OrderBy(x => x.ProgramTitle).ToList()
                        : programExports.OrderByDescending(x => x.ProgramTitle).ToList();
                case ProgramSortField.Delivery:
                    return options.SortDirection == SortDirection.Ascending
                        ? programExports.OrderBy(x => x.ProgramDelivery).ToList()
                        : programExports.OrderByDescending(x => x.ProgramDelivery).ToList();
                case ProgramSortField.College:
                    return options.SortDirection == SortDirection.Ascending
                        ? programExports.OrderBy(x => x.CollegeCode).ToList()
                        : programExports.OrderByDescending(x => x.CollegeCode).ToList();
                case ProgramSortField.Campus:
                    return options.SortDirection == SortDirection.Ascending
                        ? programExports.OrderBy(x => x.CampusCode).ToList()
                        : programExports.OrderByDescending(x => x.CampusCode).ToList();
                default:
                    return programExports.OrderBy(x => x.ProgramTitle).ToList();
            }
        }

        private async Task<Dictionary<string, string>> ColumnNames(string locale)
        {
            var allTranslations = await _translationsCache.GetTranslations(locale, Constants.Translations.ApplyAdminApi);

            return new Dictionary<string, string>
            {
                { "ApplicationCycle", allTranslations.Get("report.program.headers.application_cycle") },
                { "CollegeCode", allTranslations.Get("report.program.headers.college_code") },
                { "CampusCode", allTranslations.Get("report.program.headers.campus_code") },
                { "ProgramCode", allTranslations.Get("report.program.headers.program_code") },
                { "ProgramTitle", allTranslations.Get("report.program.headers.program") },
                { "ProgramDelivery", allTranslations.Get("report.program.headers.program_delivery_description") },
                { "ProgramType", allTranslations.Get("report.program.headers.program_type") },
                { "Promotion", allTranslations.Get("report.program.headers.promotion") },
                { "Length", allTranslations.Get("report.program.headers.length") },
                { "UnitOfMeasure", allTranslations.Get("report.program.headers.unit_of_measure_desc") },
                { "AdultTraining", allTranslations.Get("report.program.headers.adult_training_description") },
                { "ProgramSpecialCode", allTranslations.Get("report.program.headers.program_special_code") },
                { "ProgramSpecialCodeDescription", allTranslations.Get("report.program.headers.program_special_code_desc") },
                { "Credential", allTranslations.Get("report.program.headers.credential") },
                { "ApsNumber", allTranslations.Get("report.program.headers.aps_number") },
                { "StudyArea", allTranslations.Get("report.program.headers.study_area") },
                { "HighlyCompetitive", allTranslations.Get("report.program.headers.highly_competitive") },
                { "ProgramLanguage", allTranslations.Get("report.program.headers.program_language_desc") },
                { "ProgramEntryLevel", allTranslations.Get("report.program.headers.program_level_desc_code") },
                { "McuCode", allTranslations.Get("report.program.headers.mcu_code") },
                { "McuDescription", allTranslations.Get("report.program.headers.mcu_description") },
                { "MinistryApproval", allTranslations.Get("report.program.headers.ministry_approval") },
                { "Url", allTranslations.Get("report.program.headers.url") },
                { "ProgramCategory1", allTranslations.Get("report.program.headers.program_category_desc_1") },
                { "ProgramSubCategory1", allTranslations.Get("report.program.headers.program_sub_category_desc_1") },
                { "ProgramCategory2", allTranslations.Get("report.program.headers.program_category_desc_2") },
                { "ProgramSubCategory2", allTranslations.Get("report.program.headers.program_sub_category_desc_2") }
            };
        }

        private async Task<byte[]> GetSpreadSheetBytes(IList<LookupItem> allEntryLevels, IList<ProgramExport> programExports, IList<Dto.Program> dtoPrograms)
        {
            var results = new List<IDictionary<string, object>>();
            var columNames = await ColumnNames(_locale);
            foreach (var programExport in programExports)
            {
                // Find the program matching the export entry levels to check if set
                var programEntryLevels = dtoPrograms.FirstOrDefault(x => x.Id == programExport.ProgramId)?.EntryLevels;
                var entryLevels = allEntryLevels
                                    .GroupJoin(
                                            programEntryLevels,
                                            ae => ae.Id,
                                            pe => pe,
                                            (ae, pe) => new { EntryLevel = ae.Label, IsSet = !pe.FirstOrDefault().IsEmpty() ? "1" : null })
                                    .Select(x => new KeyValuePair<string, object>(x.EntryLevel, x.IsSet));

                var recordDictionary = programExport.ToDictionary(columNames);
                recordDictionary.Remove("ProgramId"); // Not shown in report
                var record = recordDictionary.ToList();

                var indexPlacement = record.FindIndex(x => x.Key == columNames.FirstOrDefault(y => y.Key == "McuCode").Value);
                record.InsertRange(indexPlacement, entryLevels);
                results.Add(record.ToDictionary(pair => pair.Key, pair => pair.Value));
            }

            using (var wb = new XLWorkbook())
            {
                var sheetName = await _translationsCache.GetTranslationValue(_locale, "report.program.worksheet", Constants.Translations.ApplyAdminApi);
                var ws = wb.AddWorksheet(results.ToDataTable(), sheetName);
                ws.SheetView.FreezeColumns(4);

                byte[] spreadsheetArray;
                using (var spreadsheetStream = new MemoryStream())
                {
                    wb.SaveAs(spreadsheetStream);
                    spreadsheetStream.Seek(0, SeekOrigin.Begin);
                    using (var ms = new MemoryStream())
                    {
                        spreadsheetStream.CopyTo(ms);
                        spreadsheetArray = ms.ToArray();
                    }
                }

                return spreadsheetArray;
            }
        }
    }
}
