using System;
using System.Collections.Generic;
using System.Linq;
using Ocas.Domestic.Apply.Admin.Models;
using Ocas.Domestic.Apply.Models;
using Ocas.Domestic.Apply.Models.Lookups;
using Ocas.Domestic.AppSettings.Extras;
using Ocas.Domestic.Enums;
using Dto = Ocas.Domestic.Models;

namespace Ocas.Domestic.Apply.Admin.Api.Services.Mappers
{
    public partial class ApiMapper
    {
        public ApplicationSummary MapApplicationSummary(Dto.ApplicationSummary applicationSummary, Dto.Contact applicant, IList<Dto.ProgramIntake> programIntakes, IList<LookupItem> applicationStatuses, IList<LookupItem> instituteTypes, IList<TranscriptTransmission> transcriptTransmissions, IList<LookupItem> offerStates, IList<LookupItem> programIntakeAvailabilities, IAppSettingsExtras appSettingsExtras)
        {
            var shoppingCartDetails = applicationSummary.ProgramChoices.Any(x => x.SupplementalFeePaid == false) ? applicationSummary.ShoppingCartDetails : null;
            var programChoices = ProgramChoices(applicationSummary.ProgramChoices, shoppingCartDetails, programIntakes, programIntakeAvailabilities);

            return new ApplicationSummary
            {
                Application = MapApplication(applicationSummary.Application),
                Offers = Offers(applicationSummary.Offers, applicationStatuses, applicationSummary.Application.Id, offerStates, appSettingsExtras),
                ProgramChoices = programChoices,
                FinancialTransactions = applicationSummary.FinancialTransactions != null ? FinancialTransactions(applicationSummary.FinancialTransactions) : null,
                TranscriptRequests = applicationSummary.TranscriptRequests != null ? TranscriptRequests(applicationSummary.TranscriptRequests, instituteTypes, transcriptTransmissions) : null,
                ShoppingCartDetails = applicationSummary.ShoppingCartDetails != null ? MapShoppingCartDetail(applicationSummary.ShoppingCartDetails, applicant, applicationSummary.Application) : null
            };
        }

        private IList<ProgramChoice> ProgramChoices(IList<Dto.ProgramChoice> choices, IList<Dto.ShoppingCartDetail> shoppingCartDetails, IList<Dto.ProgramIntake> programIntakes, IList<LookupItem> programIntakeAvailabilities)
        {
            var filterChoices = choices.Where(c => c.SequenceNumber <= Constants.ProgramChoices.MaxTotalChoices).ToList();

            if (filterChoices.Any())
            {
                return MapProgramChoices(filterChoices.OrderBy(c => c.SequenceNumber).ToList(), programIntakes, programIntakeAvailabilities, shoppingCartDetails);
            }

            return new List<ProgramChoice>();
        }

        private IList<Offer> Offers(IList<Dto.Offer> dtoOffers, IList<LookupItem> applicationStatuses, Guid applicationId, IList<LookupItem> offerStates, IAppSettingsExtras appSettingsExtras)
        {
            var paidStatusId = applicationStatuses.First(x => x.Code == Constants.ApplicationStatuses.Active).Id;
            var offers = dtoOffers.Where(x => x.ApplicationStatusId == paidStatusId);
            var appOffers = offers.Where(x => x.ApplicationId == applicationId).ToList();
            return MapOffers(appOffers, offerStates, applicationStatuses, appSettingsExtras);
        }

        private IList<FinancialTransaction> FinancialTransactions(IList<Dto.FinancialTransaction> dtoFinancialTransactions)
        {
            var result = dtoFinancialTransactions
                .Select(MapFinancialTransaction).ToList();

            var transfers = dtoFinancialTransactions
                .Where(x => x.TransactionType == TransactionType.Transfer && result.All(y => y.Id != x.Id))
                .Select(x =>
                {
                    var model = MapFinancialTransaction(x);
                    model.ApplicationId = model.OrderApplicationId;
                    return model;
                })
                .ToList();

            result.AddRange(transfers);
            return result;
        }

        private IList<TranscriptRequest> TranscriptRequests(IList<Dto.TranscriptRequest> dtoTranscriptRequests, IList<LookupItem> instituteTypes, IList<TranscriptTransmission> transcriptTransmissions)
        {
            var transcriptRequests = new List<TranscriptRequest>();
            foreach (var dtoTranscriptRequest in dtoTranscriptRequests)
            {
                transcriptRequests.Add(MapTranscriptRequest(
                    dtoTranscriptRequest,
                    instituteTypes,
                    transcriptTransmissions));
            }

            return transcriptRequests;
        }
    }
}
