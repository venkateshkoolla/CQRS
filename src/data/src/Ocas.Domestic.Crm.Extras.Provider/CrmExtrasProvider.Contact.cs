using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Ocas.Domestic.Enums;
using Ocas.Domestic.Models;

namespace Ocas.Domestic.Crm.Extras.Provider
{
    public partial class CrmExtrasProvider
    {
        public async Task<ApplicantSummary> GetApplicantSummary(GetApplicantSummaryOptions options)
        {
            var sql = $@"EXEC [dbo].[{Sprocs.ContactsGet.Sproc}] @{Sprocs.ContactsGet.Id} = @ParamApplicantId;
                         EXEC [dbo].[{Sprocs.EducationsGet.Sproc}] @{Sprocs.EducationsGet.ApplicantId} = @ParamApplicantId;
                         EXEC [dbo].[{Sprocs.SupportingDocumentsGet.Sproc}] @{Sprocs.SupportingDocumentsGet.ApplicantId} = @ParamApplicantId;
                         EXEC [dbo].[{Sprocs.TranscriptsGet.Sproc}] @{Sprocs.TranscriptsGet.ContactId} = @ParamApplicantId;
                         EXEC [dbo].[{Sprocs.TestsGet.Sproc}] @{Sprocs.TestsGet.ApplicantId} = @ParamApplicantId, @{Sprocs.TestsGet.Locale} = @ParamLocale;
                         EXEC [dbo].[{Sprocs.AcademicRecordGet.Sproc}] @{Sprocs.AcademicRecordGet.ApplicantId} = @ParamApplicantId;
                         EXEC [dbo].[{Sprocs.OntarioStudentCourseCreditsGet.Sproc}] @{Sprocs.OntarioStudentCourseCreditsGet.ApplicantId} = @ParamApplicantId;
                         EXEC [dbo].[{Sprocs.ApplicationsGet.Sproc}] @{Sprocs.ApplicationsGet.ApplicantId} = @ParamApplicantId, @{Sprocs.ApplicationsGet.ApplicationStatusId} = @ParamApplicationStatusId, @{Sprocs.ApplicationsGet.Id} = @ParamApplicationId;";

            if (options.ApplicationId != null)
            {
                sql += "SELECT @ParamApplicationId AS [ApplicationId];";

                sql += $@"EXEC [dbo].[{Sprocs.OffersGet.Sproc}] @{Sprocs.OffersGet.ApplicantId} = @ParamApplicantId, @{Sprocs.OffersGet.ApplicationId} = @ParamApplicationId;
                          EXEC [dbo].[{Sprocs.ProgramChoicesGet.Sproc}] @{Sprocs.ProgramChoicesGet.ApplicantId} = @ParamApplicantId, @{Sprocs.ProgramChoicesGet.ApplicationId} = @ParamApplicationId;";

                if (options.IncludeTranscriptRequests)
                    sql += $" EXEC [dbo].[{Sprocs.TranscriptRequestsGet.Sproc}] @{Sprocs.TranscriptRequestsGet.ApplicantId} = @ParamApplicantId, @{Sprocs.TranscriptRequestsGet.ApplicationId} = @ParamApplicationId;";

                if (options.IncludeFinancialTransactions)
                    sql += $" EXEC [dbo].[{Sprocs.FinancialTransactionsGet.Sproc}] @{Sprocs.FinancialTransactionsGet.ApplicantId} = @ParamApplicantId, @{Sprocs.FinancialTransactionsGet.ApplicationId} = @ParamApplicationId;";

                if (options.IncludeShoppingCartDetails)
                    sql += $" EXEC [dbo].[{Sprocs.ShoppingCartDetailsGet.Sproc}] @{Sprocs.ShoppingCartDetailsGet.ApplicationId} = @ParamApplicationId, @{Sprocs.ShoppingCartDetailsGet.Locale} = @ParamLocale;";
            }
            else
            {
                sql += $@"DECLARE @CursorApplicationId UNIQUEIDENTIFIER;
                          DECLARE ApplicationCursor CURSOR FOR
                              SELECT [Applications].[Id]
                              FROM [dbo].[view_Applications] [Applications]
                              WHERE [Applications].[ApplicantId] = @ParamApplicantId AND [StateCode] = 0 AND [Code] = 'A';
                          
                          OPEN ApplicationCursor;
                          FETCH NEXT FROM ApplicationCursor INTO @CursorApplicationId;
                          WHILE @@FETCH_STATUS = 0
                             BEGIN
                                 SELECT @CursorApplicationId AS [ApplicationId];
                                 EXEC [dbo].[{Sprocs.OffersGet.Sproc}] @{Sprocs.OffersGet.ApplicantId} = @ParamApplicantId, @{Sprocs.OffersGet.ApplicationId} = @CursorApplicationId;
                                 EXEC [dbo].[{Sprocs.ProgramChoicesGet.Sproc}] @{Sprocs.ProgramChoicesGet.ApplicantId} = @ParamApplicantId, @{Sprocs.ProgramChoicesGet.ApplicationId} = @CursorApplicationId;";

                if (options.IncludeTranscriptRequests)
                    sql += $"    EXEC [dbo].[{Sprocs.TranscriptRequestsGet.Sproc}] @{Sprocs.TranscriptRequestsGet.ApplicantId} = @ParamApplicantId, @{Sprocs.TranscriptRequestsGet.ApplicationId} = @CursorApplicationId;";

                if (options.IncludeFinancialTransactions)
                    sql += $"    EXEC [dbo].[{Sprocs.FinancialTransactionsGet.Sproc}] @{Sprocs.FinancialTransactionsGet.ApplicantId} = @ParamApplicantId, @{Sprocs.FinancialTransactionsGet.ApplicationId} = @CursorApplicationId;";

                if (options.IncludeShoppingCartDetails)
                    sql += $"    EXEC [dbo].[{Sprocs.ShoppingCartDetailsGet.Sproc}] @{Sprocs.ShoppingCartDetailsGet.ApplicationId} = @CursorApplicationId, @{Sprocs.ShoppingCartDetailsGet.Locale} = @ParamLocale;";

                sql += @"        FETCH NEXT FROM ApplicationCursor INTO @CursorApplicationId;
                             END;
                         CLOSE ApplicationCursor;
                         DEALLOCATE ApplicationCursor;";
            }

            var parameters = new
            {
                @ParamApplicantId = options.ApplicantId,
                @ParamLocale = options.Locale,
                @ParamApplicationStatusId = options.ApplicationStatusId,
                @ParamApplicationId = options.ApplicationId
            };

            var results = await Connection.QueryMultipleAsync(
                sql,
                parameters,
                commandType: CommandType.Text,
                commandTimeout: _commandTimeout);

            // Below sequence of execution must match with the above sql sequence
            var applicant = results.Read(
                new Func<Contact, Address, Contact>((contact, address) =>
                {
                    contact.MailingAddress = address;
                    return contact;
                }),
                splitOn: "CountryId").AsList().FirstOrDefault();
            var educations = results.Read<Education>().AsList();
            var supportingDocs = results.Read<SupportingDocument>().AsList();
            var transcripts = results.Read<Transcript>().AsList();
            var testDictionary = new Dictionary<Guid, Test>();
            var testMapFunc = new Func<Test, TestDetail, Test>((test, detail) =>
            {
                if (!testDictionary.TryGetValue(test.Id, out var testEntry))
                {
                    testEntry = test;
                    testEntry.Details = new List<TestDetail>();
                    testDictionary.Add(testEntry.Id, testEntry);
                }

                if (detail != null)
                {
                    testEntry.Details.Add(detail);
                }

                return testEntry;
            });
            results.Read(testMapFunc);
            var tests = testDictionary.Values.AsList();
            var academicRecords = results.Read<AcademicRecord>().AsList();
            var ontarioStudentCourseCredits = results.Read<OntarioStudentCourseCredit>().AsList();

            var applications = results.Read<Application>().AsList().ToDictionary(x => x.Id, x => x);
            var applicationSummaries = new List<ApplicationSummary>(applications.Count);

            for (var i = 0; i < applications.Count; i++)
            {
                var applicationId = results.ReadFirst<Guid>();
                var application = applications[applicationId];

                var offers = results.Read<Offer>().AsList();
                var programChoices = results.Read<ProgramChoice>().AsList();

                IList<TranscriptRequest> transcriptRequests = null;
                if (options.IncludeTranscriptRequests)
                    transcriptRequests = results.Read<TranscriptRequest>().AsList();

                IList<FinancialTransaction> financialTransactions = null;
                if (options.IncludeFinancialTransactions)
                {
                    financialTransactions = results.Read(new Func<FinancialTransaction, Receipt, FinancialTransaction>((master, detail) =>
                    {
                        master.Receipt = detail;

                        return master;
                    })).AsList();
                }

                IList<ShoppingCartDetail> shoppingCartDetails = null;
                if (options.IncludeShoppingCartDetails)
                    shoppingCartDetails = results.Read<ShoppingCartDetail>().AsList();

                var applicationSummary = new ApplicationSummary
                {
                    Application = application,
                    Offers = offers,
                    ProgramChoices = programChoices,
                    TranscriptRequests = transcriptRequests,
                    FinancialTransactions = financialTransactions,
                    ShoppingCartDetails = shoppingCartDetails
                };

                applicationSummaries.Add(applicationSummary);
            }

            return new ApplicantSummary
            {
                Contact = applicant,
                Educations = educations,
                SupportingDocuments = supportingDocs,
                Transcripts = transcripts,
                AcademicRecords = academicRecords,
                OntarioStudentCourseCredits = ontarioStudentCourseCredits,
                Tests = tests,
                ApplicationSummaries = applicationSummaries
            };
        }

        public Task<ApplicantCompletedSteps> GetCompletedSteps(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ContactCompletedSteps.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<ApplicantCompletedSteps>(Sprocs.ContactCompletedSteps.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public async Task<Contact> GetContact(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ContactsGet.StateCode, State.Active },
                { Sprocs.ContactsGet.StatusCode, ContactStatusCode.Active },
                { Sprocs.ContactsGet.Id, id }
            };

            var result = await Connection.QueryAsync<Contact, Address, Contact>(
                Sprocs.ContactsGet.Sproc,
                (contact, address) =>
                {
                    contact.MailingAddress = address;
                    return contact;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "CountryId");

            return result.FirstOrDefault();
        }

        public async Task<Contact> GetContact(string userName, string subjectId)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ContactsGet.StateCode, State.Active },
                { Sprocs.ContactsGet.StatusCode, ContactStatusCode.Active },
                { Sprocs.ContactsGet.UserName, userName },
                { Sprocs.ContactsGet.SubjectId, subjectId }
            };

            var result = await Connection.QueryAsync<Contact, Address, Contact>(
                Sprocs.ContactsGet.Sproc,
                (contact, address) =>
                {
                    contact.MailingAddress = address;
                    return contact;
                },
                parameters,
                commandType: CommandType.StoredProcedure,
                commandTimeout: _commandTimeout,
                splitOn: "CountryId");

            return result.FirstOrDefault();
        }

        public Task<string> GetContactSubjectId(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ContactGetSubjectId.Id, id }
            };

            return Connection.QueryFirstOrDefaultAsync<string>(Sprocs.ContactGetSubjectId.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<bool> IsActive(Guid id)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ContactIsActive.Id, id }
            };

            return Connection.ExecuteScalarAsync<bool>(Sprocs.ContactIsActive.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<bool> IsDuplicateContact(Guid id, string emailAddress)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ContactIsDuplicateEmail.Id, id },
                { Sprocs.ContactIsDuplicateEmail.EmailAddress, emailAddress }
            };

            return Connection.ExecuteScalarAsync<bool>(Sprocs.ContactIsDuplicateEmail.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<bool> IsDuplicateContact(Guid id, string firstName, string lastName, DateTime birthDate)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ContactIsDuplicateDetails.Id, id },
                { Sprocs.ContactIsDuplicateDetails.FirstName, firstName },
                { Sprocs.ContactIsDuplicateDetails.LastName, lastName },
                { Sprocs.ContactIsDuplicateDetails.BirthDate, birthDate }
            };

            return Connection.ExecuteScalarAsync<bool>(Sprocs.ContactIsDuplicateDetails.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<bool> IsDuplicateOen(Guid id, string oen)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.ContactIsDuplicateOen.Id, id },
                { Sprocs.ContactIsDuplicateOen.Oen, oen }
            };

            return Connection.ExecuteScalarAsync<bool>(Sprocs.ContactIsDuplicateOen.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }

        public Task<bool> CanAccessApplicant(Guid applicantId, string partnerId, UserType userType)
        {
            var parameters = new Dictionary<string, object>
            {
                { Sprocs.CanAccessApplicantGet.ApplicantId, applicantId },
                { Sprocs.CanAccessApplicantGet.PartnerId, partnerId },
                { Sprocs.CanAccessApplicantGet.UserType, (int)userType }
            };

            return Connection.ExecuteScalarAsync<bool>(Sprocs.CanAccessApplicantGet.Sproc, parameters, commandType: CommandType.StoredProcedure, commandTimeout: _commandTimeout);
        }
    }
}
