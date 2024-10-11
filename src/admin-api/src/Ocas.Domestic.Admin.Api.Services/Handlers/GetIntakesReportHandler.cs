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
using Ocas.Domestic.Admin.Enums;
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
    public class GetIntakesReportHandler : IRequestHandler<GetIntakesReport, BinaryDocument>
    {
        private readonly ILogger<GetIntakesReportHandler> _logger;
        private readonly IDomesticContext _domesticContext;
        private readonly ILookupsCache _lookupsCache;
        private readonly string _locale;
        private readonly IApiMapper _apiMapper;
        private readonly ITranslationsCache _translationsCache;

        public GetIntakesReportHandler(
            ILogger<GetIntakesReportHandler> logger,
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

        public async Task<BinaryDocument> Handle(GetIntakesReport request, CancellationToken cancellationToken)
        {
            var spreadSheetArray = Array.Empty<byte>();
            var options = new Dto.GetProgramIntakeOptions
            {
                ApplicationCycleId = request.ApplicationCycleId,
                CollegeId = request.CollegeId,
                CampusId = request.Params.CampusId,
                ProgramCode = request.Params.ProgramCode,
                ProgramTitle = request.Params.ProgramTitle,
                ProgramDeliveryId = request.Params.DeliveryId,
                FromDate = request.Params.StartDate
            };

            var results = await _domesticContext.GetProgramIntakes(options);
            var collegeApplicationCycles = await _lookupsCache.GetCollegeApplicationCycles();
            var collegeApplicationCycle = collegeApplicationCycles.FirstOrDefault(c => c.CollegeId == request.CollegeId && c.MasterId == request.ApplicationCycleId);

            if (results.Any())
            {
                var allEntryLevels = await _lookupsCache.GetEntryLevels(_locale);
                var intakeExports = await GetIntakeExports(results, collegeApplicationCycle, allEntryLevels, request.Params);
                spreadSheetArray = await GetSpreadSheetBytes(allEntryLevels, intakeExports, results);
            }

            var fileName = await _translationsCache.GetTranslationValue(_locale, "report.intake.workbook", Constants.Translations.ApplyAdminApi);
            return new BinaryDocument
            {
                Name = $"{fileName} {collegeApplicationCycle.Name}.xlsx",
                MimeType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                CreatedDate = DateTime.Now,
                CreatedBy = request.User.GetDisplayName(),
                Data = spreadSheetArray
            };
        }

        private async Task<IList<IntakeExport>> GetIntakeExports(IList<Dto.ProgramIntake> results, CollegeApplicationCycle collegeApplicationCycle, IList<LookupItem> allEntryLevels, GetIntakesOptions options)
        {
            var colleges = await _lookupsCache.GetColleges(_locale);
            var campuses = await _lookupsCache.GetCampuses();
            var programDeliveries = await _lookupsCache.GetProgramDeliveries(_locale);
            var programIntakeAvailabilities = await _lookupsCache.GetProgramIntakeAvailabilities(_locale);
            var programIntakeStatuses = await _lookupsCache.GetIntakeStatuses(_locale);
            var intakeExpiryActions = await _lookupsCache.GetIntakeExpiryActions(_locale);
           var intakeExports = _apiMapper.MapProgramIntakeExports(results, colleges, campuses, collegeApplicationCycle, programDeliveries, programIntakeAvailabilities, programIntakeStatuses, intakeExpiryActions, allEntryLevels);

            switch (options.SortBy)
            {
                case IntakeSortField.Code:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakeExports.OrderBy(x => x.ProgramCode).ToList()
                        : intakeExports.OrderByDescending(x => x.ProgramCode).ToList();
                case IntakeSortField.Title:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakeExports.OrderBy(x => x.ProgramTitle).ToList()
                        : intakeExports.OrderByDescending(x => x.ProgramTitle).ToList();
                case IntakeSortField.Delivery:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakeExports.OrderBy(x => x.ProgramDelivery).ToList()
                        : intakeExports.OrderByDescending(x => x.ProgramDelivery).ToList();
                case IntakeSortField.StartDate:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakeExports.OrderBy(x => x.StartDate).ToList()
                        : intakeExports.OrderByDescending(x => x.StartDate).ToList();
                case IntakeSortField.Campus:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakeExports.OrderBy(x => x.CampusCode).ToList()
                        : intakeExports.OrderByDescending(x => x.CampusCode).ToList();
                case IntakeSortField.Status:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakeExports.OrderBy(x => x.ProgramIntakeStatus).ToList()
                        : intakeExports.OrderByDescending(x => x.ProgramIntakeStatus).ToList();
                case IntakeSortField.Availability:
                    return options.SortDirection == SortDirection.Ascending
                        ? intakeExports.OrderBy(x => x.ProgramIntakeAvailability).ToList()
                        : intakeExports.OrderByDescending(x => x.ProgramIntakeAvailability).ToList();
                default:
                    return intakeExports.OrderBy(x => x.ProgramTitle).ToList();
            }
        }

        private async Task<byte[]> GetSpreadSheetBytes(IList<LookupItem> allEntryLevels, IList<IntakeExport> intakeExports, IList<Dto.ProgramIntake> dtoProgramIntakes)
        {
            var results = new List<IDictionary<string, object>>();
            var columNames = await ColumnNames(_locale);
            foreach (var intakeExport in intakeExports)
            {
                // Find the programIntake matching the export entry levels to check if set
                var intakeEntryLevels = dtoProgramIntakes.FirstOrDefault(x => x.Id == intakeExport.IntakeId)?.EntryLevels;
                var entryLevels = allEntryLevels
                                    .GroupJoin(
                                            intakeEntryLevels,
                                            ae => ae.Id,
                                            pe => pe,
                                            (ae, pe) => new { EntryLevel = ae.Label, IsSet = !pe.FirstOrDefault().IsEmpty() ? "1" : null })
                                    .Select(x => new KeyValuePair<string, object>(x.EntryLevel, x.IsSet));

                var recordDictionary = intakeExport.ToDictionary(columNames);
                recordDictionary.Remove("IntakeId");
                var record = recordDictionary.ToList();

                var indexPlacement = record.FindIndex(x => x.Key == columNames.FirstOrDefault(y => y.Key == "DefaultSemesterOverride").Value) + 1;
                record.InsertRange(indexPlacement, entryLevels);
                results.Add(record.ToDictionary(pair => pair.Key, pair => pair.Value));
            }

            using (var wb = new XLWorkbook())
            {
                var sheetName = await _translationsCache.GetTranslationValue(_locale, "report.intake.worksheet", Constants.Translations.ApplyAdminApi);
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

        private async Task<Dictionary<string, string>> ColumnNames(string locale)
        {
            var allTranslations = await _translationsCache.GetTranslations(locale, Constants.Translations.ApplyAdminApi);

            return new Dictionary<string, string>
            {
                { "ApplicationCycle", allTranslations.Get("report.intake.headers.application_cycle") },
                { "CollegeCode", allTranslations.Get("report.intake.headers.college_code") },
                { "CampusCode", allTranslations.Get("report.intake.headers.campus_code") },
                { "ProgramCode", allTranslations.Get("report.intake.headers.program_code") },
                { "ProgramTitle", allTranslations.Get("report.intake.headers.program") },
                { "ProgramDelivery", allTranslations.Get("report.intake.headers.program_delivery_description") },
                { "StartDate",  allTranslations.Get("report.intake.headers.start_date") },
                { "ProgramIntakeAvailability",  allTranslations.Get("report.intake.headers.program_intake_availability") },
                { "ProgramIntakeStatus",  allTranslations.Get("report.intake.headers.program_intake_status") },
                { "ExpiryDate",  allTranslations.Get("report.intake.headers.expiry_date") },
                { "HasSemesterOverride",  allTranslations.Get("report.intake.headers.semester_override") },
                { "DefaultSemesterOverride",  allTranslations.Get("report.intake.headers.default_semester") }
            };
        }
    }
}
