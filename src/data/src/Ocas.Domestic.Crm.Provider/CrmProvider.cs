using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Ocas.Domestic.Crm.Entities;
using Ocas.Domestic.Transactions;
using Polly;
using Task = System.Threading.Tasks.Task;

namespace Ocas.Domestic.Crm.Provider
{
    public class CrmProvider : IResourceManager, IDisposable
    {
        private static readonly AsyncPolicy _retryPolicy = Policy
            .Handle<MessageSecurityException>()
            .RetryAsync(onRetry: (exception, i, context) =>
            {
                if (!context.TryGetLogger(out var logger)) return;

                if (exception != null)
                {
                    logger.LogError(exception, $"[{context.CorrelationId}] Retry {i} of {context.OperationKey} using {context.PolicyKey}, due to {exception.Message}.", context.CorrelationId, context.OperationKey, context.PolicyKey);
                }
                else
                {
                    logger.LogError($"[{context.CorrelationId}] Retry {i} of {context.OperationKey} using {context.PolicyKey}.", context.CorrelationId, context.OperationKey, context.PolicyKey);
                }
            });

        private readonly string _connectionString;
        private readonly string _wcfServiceUrl;
        private readonly ILogger _logger;
        private TransactionManager _transactionManager;
        private bool _disposedValue;

        public DomesticCrmServiceContext ServiceContext { get; private set; }
        public OrganizationServiceProxy ServiceProxy { get; private set; }

        public IQueryable<ocaslr_aboriginalstatus> AboriginalStatuses => ServiceContext.ocaslr_aboriginalstatusSet;
        public IQueryable<ocaslr_academicdata> AcademicData => ServiceContext.ocaslr_academicdataSet;
        public IQueryable<ocaslr_accountstatus> AccountStatuses => ServiceContext.ocaslr_accountstatusSet;
        public IQueryable<CustomerAddress> Addresses => ServiceContext.CustomerAddressSet;
        public IQueryable<ocaslr_addresstype> AddressTypes => ServiceContext.ocaslr_addresstypeSet;
        public IQueryable<ocaslr_adulttraining> AdultTraining => ServiceContext.ocaslr_adulttrainingSet;
        public IQueryable<ocaslr_applicantactivity> ApplicantActivities => ServiceContext.ocaslr_applicantactivitySet;
        public IQueryable<ocaslr_peterequestlog> ApplicantTranscriptRequestLog => ServiceContext.ocaslr_peterequestlogSet;
        public IQueryable<ocaslr_applicationcycle> ApplicationCycle => ServiceContext.ocaslr_applicationcycleSet;
        public IQueryable<ocaslr_applicationcyclestatus> ApplicationCycleStatuses => ServiceContext.ocaslr_applicationcyclestatusSet;
        public IQueryable<ocaslr_applicantmessage> ApplicantMessages => ServiceContext.ocaslr_applicantmessageSet;
        public IQueryable<ocaslr_application> Applications => ServiceContext.ocaslr_applicationSet;
        public IQueryable<Ocaslr_applicationstatus> ApplicationStatus => ServiceContext.Ocaslr_applicationstatusSet;
        public IQueryable<ocaslr_appliedstatus> AppliedStatuses => ServiceContext.ocaslr_appliedstatusSet;
        public IQueryable<custom_audit_detail> AuditDetails => ServiceContext.custom_audit_detailSet;
        public IQueryable<custom_audit> AuditLogs => ServiceContext.custom_auditSet;
        public IQueryable<ocaslr_basisforadmission> BasisforAdmissions => ServiceContext.ocaslr_basisforadmissionSet;
        public IQueryable<BusinessUnit> BusinessUnits => ServiceContext.BusinessUnitSet;
        public IQueryable<ocaslr_programcategory> Categories => ServiceContext.ocaslr_programcategorySet;
        public IQueryable<ocaslr_city> Cities => ServiceContext.ocaslr_citySet;
        public IQueryable<ocaslr_collegeapplicationcycle> CollegeApplicationCycles => ServiceContext.ocaslr_collegeapplicationcycleSet;
        public IQueryable<ocaslr_collegeapplicationcyclestatus> CollegeApplicationCycleStatuses => ServiceContext.ocaslr_collegeapplicationcyclestatusSet;
        public IQueryable<ocaslr_collegeinformation> CollegeInformation => ServiceContext.ocaslr_collegeinformationSet;
        public IQueryable<Ocaslr_comments> Comments => ServiceContext.Ocaslr_commentsSet;
        public IQueryable<Ocaslr_commentswhat> CommentsWhats => ServiceContext.Ocaslr_commentswhatSet;
        public IQueryable<Ocaslr_commentswho> CommentsWhos => ServiceContext.Ocaslr_commentswhoSet;
        public IQueryable<ocaslr_communityinvolvement> CommunityInvolvement => ServiceContext.ocaslr_communityinvolvementSet;
        public IQueryable<ocaslr_complete> Complete => ServiceContext.ocaslr_completeSet;
        public IQueryable<Contact> Contacts => ServiceContext.ContactSet;
        public IQueryable<ocaslr_country> Countries => ServiceContext.ocaslr_countrySet;
        public IQueryable<ocaslr_coursedelivery> CourseDeliveries => ServiceContext.ocaslr_coursedeliverySet;
        public IQueryable<ocaslr_courselanguage> CourseLanguages => ServiceContext.ocaslr_courselanguageSet;
        public IQueryable<ocaslr_coursestatus> CourseStatuses => ServiceContext.ocaslr_coursestatusSet;
        public IQueryable<ocaslr_coursetype> CourseTypes => ServiceContext.ocaslr_coursetypeSet;
        public IQueryable<ocaslr_credential> Credentials => ServiceContext.ocaslr_credentialSet;
        public IQueryable<ocaslr_current> Current => ServiceContext.ocaslr_currentSet;
        public IQueryable<ocaslr_curriculum> Curriculums => ServiceContext.ocaslr_curriculumSet;
        public IQueryable<Ocaslr_documentdistributionrecord> DocumentDistributionRecords => ServiceContext.Ocaslr_documentdistributionrecordSet;
        public IQueryable<ocaslr_documentype> DocumentTypes => ServiceContext.ocaslr_documentypeSet;
        public IQueryable<Ocaslr_editexception> EditExceptions => ServiceContext.Ocaslr_editexceptionSet;
        public IQueryable<ocaslr_education> EducationRecords => ServiceContext.ocaslr_educationSet;
        public IQueryable<Template> EmailTemplates => ServiceContext.TemplateSet;
        public IQueryable<ocaslr_entrylevel> EntryLevels => ServiceContext.ocaslr_entrylevelSet;
        public IQueryable<Ocaslr_etmsfileformat> eTMSFileFormats => ServiceContext.Ocaslr_etmsfileformatSet;
        public IQueryable<Ocaslr_etmsnotificationsubscription> eTMSNotificationSubscriptions => ServiceContext.Ocaslr_etmsnotificationsubscriptionSet;
        public IQueryable<Ocaslr_etmspartnerrole> eTMSPartnerRoles => ServiceContext.Ocaslr_etmspartnerroleSet;
        public IQueryable<Ocaslr_etmsdocumentsubtype> eTMSTransactionSubTypes => ServiceContext.Ocaslr_etmsdocumentsubtypeSet;
        public IQueryable<Ocaslr_etmsdocumenttype> eTMSTransactionTypes => ServiceContext.Ocaslr_etmsdocumenttypeSet;
        public IQueryable<Ocaslr_etmstranscriptfileupload> eTMSTranscriptFileUploads => ServiceContext.Ocaslr_etmstranscriptfileuploadSet;
        public IQueryable<ocaslr_etmstranscriptrequest_destination> eTMSTranscriptRequestDestinations => ServiceContext.ocaslr_etmstranscriptrequest_destinationSet;
        public IQueryable<Ocaslr_etmstranscriptrequestprocess> eTMSTranscriptRequestProcesses => ServiceContext.Ocaslr_etmstranscriptrequestprocessSet;
        public IQueryable<Ocaslr_etmstranscriptrequestprocesstransaction> eTMSTranscriptRequestProcessTransactions => ServiceContext.Ocaslr_etmstranscriptrequestprocesstransactionSet;
        public IQueryable<Ocaslr_etmstranscriptrequest> eTMSTranscriptRequests => ServiceContext.Ocaslr_etmstranscriptrequestSet;
        public IQueryable<Ocaslr_etmstranscript> eTMSTranscripts => ServiceContext.Ocaslr_etmstranscriptSet;
        public IQueryable<Ocaslr_etmstranscripttype> eTMSTranscriptTypes => ServiceContext.Ocaslr_etmstranscripttypeSet;
        public IQueryable<ocaslr_expiryaction> ExpiryActions => ServiceContext.ocaslr_expiryactionSet;
        public IQueryable<ocaslr_financialtransaction> FinancialTransactions => ServiceContext.ocaslr_financialtransactionSet;
        public IQueryable<ocaslr_firstgenerationapplicant> FirstGenerationApplicants => ServiceContext.ocaslr_firstgenerationapplicantSet;
        public IQueryable<ocaslr_firstlanguage> FirstLanguages => ServiceContext.ocaslr_firstlanguageSet;
        public IQueryable<ocaslr_gendercode> GenderCodes => ServiceContext.ocaslr_gendercodeSet;
        public IQueryable<Ocaslr_ocasoenstatus> GetOCASOENStatus => ServiceContext.Ocaslr_ocasoenstatusSet;
        public IQueryable<Ocaslr_graderecordstatus> GradeRecordStatus => ServiceContext.Ocaslr_graderecordstatusSet;
        public IQueryable<ocaslr_gradetype> GradeTypes => ServiceContext.ocaslr_gradetypeSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_highesteducation> HighestEducations => ServiceContext.ocaslr_highesteducationSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_highlycompetitive> HighlyCompetitive => ServiceContext.ocaslr_highlycompetitiveSet;
        public IQueryable<ocaslr_highskillsmajor> HighSkillsMajors => ServiceContext.ocaslr_highskillsmajorSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_institute> Institutes => ServiceContext.ocaslr_instituteSet;
        public IQueryable<ocaslr_institutetype> InstituteTypes => ServiceContext.ocaslr_institutetypeSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_internationalcreditassessment> InternationalCreditAssessments => ServiceContext.ocaslr_internationalcreditassessmentSet;
        public IQueryable<ocaslr_internationalcreditassesmentstatus> InternationalCreditAssessmentStatus => ServiceContext.ocaslr_internationalcreditassesmentstatusSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_jobexperience> JobExperiences => ServiceContext.ocaslr_jobexperienceSet;
        public IQueryable<ocaslr_languageofinstruction> LanguageofInstruction => ServiceContext.ocaslr_languageofinstructionSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_language> Languages => ServiceContext.ocaslr_languageSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_lastgradecompleted> LastGradeCompleted => ServiceContext.ocaslr_lastgradecompletedSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_levelachieved> LevelAchieved => ServiceContext.ocaslr_levelachievedSet;
        public IQueryable<ocaslr_levelofstudies> LevelofStudies => ServiceContext.ocaslr_levelofstudiesSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_literacytest> LiteracyTest => ServiceContext.ocaslr_literacytestSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_mcucode> MCUCodes => ServiceContext.ocaslr_mcucodeSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_membershiptype> MembershipTypes => ServiceContext.ocaslr_membershiptypeSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_ministryapproval> MinistryApproval => ServiceContext.ocaslr_ministryapprovalSet;
        public IQueryable<ocaslr_month> Months => ServiceContext.ocaslr_monthSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<Annotation> Note => ServiceContext.AnnotationSet;
        public IQueryable<ocaslr_ocfileerrormessage> OCFileErrorMessages => ServiceContext.ocaslr_ocfileerrormessageSet;
        public IQueryable<ocaslr_ocfileprocessdetail> OCFileProcessDetails => ServiceContext.ocaslr_ocfileprocessdetailSet;
        public IQueryable<ocaslr_ocfileprocessheader> OCFileProcessHeaders => ServiceContext.ocaslr_ocfileprocessheaderSet;
        public IQueryable<ocaslr_offeracceptance> OfferAcceptances => ServiceContext.ocaslr_offeracceptanceSet;
        public IQueryable<ocaslr_offer> Offers => ServiceContext.ocaslr_offerSet;
        public IQueryable<ocaslr_offerstate> OfferStates => ServiceContext.ocaslr_offerstateSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_offerstatus> OfferStatuses => ServiceContext.ocaslr_offerstatusSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_offerstudymethod> OfferStudyMethods => ServiceContext.ocaslr_offerstudymethodSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<Ocaslr_offertype> OfferTypes => ServiceContext.Ocaslr_offertypeSet;
        public IQueryable<ocaslr_official> Officials => ServiceContext.ocaslr_officialSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_officialsignature> OfficialSignatures => ServiceContext.ocaslr_officialsignatureSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_ontariohighschoolcoursecodes> OntarioHighSchoolCourseCodes => ServiceContext.ocaslr_ontariohighschoolcoursecodesSet;
        public IQueryable<ocaslr_ontariostudentcoursecredit> OntarioStudentCourseCredit => ServiceContext.ocaslr_ontariostudentcoursecreditSet;
        public IQueryable<SalesOrderDetail> OrderItems => ServiceContext.SalesOrderDetailSet;
        public IQueryable<SalesOrder> Orders => ServiceContext.SalesOrderSet;
        public IQueryable<ocaslr_original> Originals => ServiceContext.ocaslr_originalSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<Ocaslr_ostnotes> OSTNotes => ServiceContext.Ocaslr_ostnotesSet;
        public IQueryable<ocaslr_partneridentity> PartnerIdentities => ServiceContext.ocaslr_partneridentitySet;
        public IQueryable<Account> Partners => ServiceContext.AccountSet;
        public IQueryable<ocaslr_paymentmethod> PaymentMethods => ServiceContext.ocaslr_paymentmethodSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_paymentresult> PaymentResult => ServiceContext.ocaslr_paymentresultSet;
        public IQueryable<ocaslr_paymenttransactiondetail> PaymentTransactionDetails => ServiceContext.ocaslr_paymenttransactiondetailSet;
        public IQueryable<ocaslr_preferredcorrespondencemethod> PreferredCorrespondenceMethods => ServiceContext.ocaslr_preferredcorrespondencemethodSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_preferredlanguage> PreferredLanguages => ServiceContext.ocaslr_preferredlanguageSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_preferredsponsoragency> PreferredSponsorAgencies => ServiceContext.ocaslr_preferredsponsoragencySet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_privacystatement> PrivacyStatements => ServiceContext.ocaslr_privacystatementSet;
        public IQueryable<Product> Products => ServiceContext.ProductSet;
        public IQueryable<ocaslr_programchoice> ProgramChoice => ServiceContext.ocaslr_programchoiceSet;
        public IQueryable<ocaslr_offerstudymethod> ProgramDelivery => ServiceContext.ocaslr_offerstudymethodSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_programintakeavailability> ProgramIntakeAvailabilities => ServiceContext.ocaslr_programintakeavailabilitySet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_programintakeentrysemester> ProgramIntakeEntrySemesters => ServiceContext.ocaslr_programintakeentrysemesterSet;
        public IQueryable<ocaslr_programintake> ProgramIntakes => ServiceContext.ocaslr_programintakeSet;
        public IQueryable<ocaslr_programintakestatus> ProgramIntakeStatuses => ServiceContext.ocaslr_programintakestatusSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_programlanguage> ProgramLanguages => ServiceContext.ocaslr_programlanguageSet;
        public IQueryable<ocaslr_programlevel> ProgramLevel => ServiceContext.ocaslr_programlevelSet;
        public IQueryable<ocaslr_program> Programs => ServiceContext.ocaslr_programSet.OrderBy(o => o.ocaslr_sortorder);
        public IQueryable<ocaslr_programspecialcode> ProgramSpecialCodes => ServiceContext.ocaslr_programspecialcodeSet;
        public IQueryable<ocaslr_programtype> ProgramTypes => ServiceContext.ocaslr_programtypeSet;
        public IQueryable<ocaslr_promotion> Promotions => ServiceContext.ocaslr_promotionSet;
        public IQueryable<ocaslr_provincestate> ProvinceStates => ServiceContext.ocaslr_provincestateSet;
        public IQueryable<Ocaslr_scholarship> Scholarships => ServiceContext.Ocaslr_scholarshipSet;
        public IQueryable<ocaslr_schoolstatus> SchoolStatuses => ServiceContext.ocaslr_schoolstatusSet;
        public IQueryable<ocaslr_schooltype> SchoolTypes => ServiceContext.ocaslr_schooltypeSet;
        public IQueryable<ocaslr_secondconsentapplicant> SecondConsentApplicants => ServiceContext.ocaslr_secondconsentapplicantSet;
        public IQueryable<OpportunityProduct> ShoppingCartItems => ServiceContext.OpportunityProductSet;
        public IQueryable<Opportunity> ShoppingCarts => ServiceContext.OpportunitySet;
        public IQueryable<ocaslr_shsmcompletion> SHSMCompletion => ServiceContext.ocaslr_shsmcompletionSet;
        public IQueryable<ocaslr_source> Sources => ServiceContext.ocaslr_sourceSet;
        public IQueryable<ocaslr_statusincanada> StatusesinCanada => ServiceContext.ocaslr_statusincanadaSet;
        public IQueryable<ocaslr_statusofvisa> StatusofVisas => ServiceContext.ocaslr_statusofvisaSet;
        public IQueryable<ocaslr_studyarea> StudyAreas => ServiceContext.ocaslr_studyareaSet;
        public IQueryable<ocaslr_programsubcategory> SubCategories => ServiceContext.ocaslr_programsubcategorySet;
        public IQueryable<ocaslr_supportingdocument> SupportingDocuments => ServiceContext.ocaslr_supportingdocumentSet;
        public IQueryable<ocaslr_systemconfiguration> SystemConfigurations => ServiceContext.ocaslr_systemconfigurationSet;
        public IQueryable<ocaslr_temporary> TemporaryLicenses => ServiceContext.ocaslr_temporarySet;
        public IQueryable<ocaslr_testdetail> TestDetails => ServiceContext.ocaslr_testdetailSet;
        public IQueryable<ocaslr_test> Tests => ServiceContext.ocaslr_testSet;
        public IQueryable<ocaslr_testtype> TestTypes => ServiceContext.ocaslr_testtypeSet;
        public IQueryable<Ocaslr_timezone> TimeZones => ServiceContext.Ocaslr_timezoneSet;
        public IQueryable<ocaslr_title> Titles => ServiceContext.ocaslr_titleSet;
        public IQueryable<Ocaslr_transcriptrequestexception> TranscriptRequestExceptions => ServiceContext.Ocaslr_transcriptrequestexceptionSet;
        public IQueryable<ocaslr_transcriptrequest> TranscriptRequests => ServiceContext.ocaslr_transcriptrequestSet;
        public IQueryable<ocaslr_transcriptrequeststatus> TranscriptRequestStatuses => ServiceContext.ocaslr_transcriptrequeststatusSet;
        public IQueryable<ocaslr_transcript> Transcripts => ServiceContext.ocaslr_transcriptSet;
        public IQueryable<ocaslr_transcriptsource> TranscriptSources => ServiceContext.ocaslr_transcriptsourceSet;
        public IQueryable<ocaslr_transcripttransmission> TranscriptTransmissions => ServiceContext.ocaslr_transcripttransmissionSet;
        public IQueryable<ocaslr_transmissiondetail> TransmissionDetails => ServiceContext.ocaslr_transmissiondetailSet;
        public IQueryable<ocaslr_transmissionerrormessage> TransmissionErrors => ServiceContext.ocaslr_transmissionerrormessageSet;
        public IQueryable<ocaslr_transmissionheader> TransmissionHeaders => ServiceContext.ocaslr_transmissionheaderSet;
        public IQueryable<ocaslr_transmissionstatus> TransmissionStatuses => ServiceContext.ocaslr_transmissionstatusSet;
        public IQueryable<ocaslr_transmissionsummary> TransmissionSummaryes => ServiceContext.ocaslr_transmissionsummarySet;
        public IQueryable<ocaslr_type> Types => ServiceContext.ocaslr_typeSet;
        public IQueryable<ocaslr_unitofmeasure> UnitsofMeasure => ServiceContext.ocaslr_unitofmeasureSet;
        public IQueryable<ocaslr_voucher> Vouchers => ServiceContext.ocaslr_voucherSet;
        public IQueryable<SystemUser> Users => ServiceContext.SystemUserSet;
        public IQueryable<ocaslr_program_entrylevels> ProgramEntryLevels => ServiceContext.ocaslr_program_entrylevelsSet;

        /// <summary>
        /// Create an entity.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="crmEntity">Crm entity being created.</param>
        /// <exception cref="ArgumentNullException"><paramref name="crmEntity"/> is <c>null</c>.</exception>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<Guid> CreateEntity(Entity crmEntity)
        {
            EnlistInTransaction();

            if (crmEntity == null) throw new ArgumentNullException(nameof(crmEntity));

            if (crmEntity.Id == default(Guid))
            {
                crmEntity.Id = Guid.NewGuid();
            }

            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceContext.CreateEntityAsync(crmEntity), context);
        }

        /// <summary>
        /// Update an entity.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="crmEntity">Crm entity being updated.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="crmEntity"/> is <c>null</c>.</exception>
        public Task UpdateEntity(Entity crmEntity)
        {
            EnlistInTransaction();
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceContext.UpdateEntityAsync(crmEntity), context);
        }

        /// <summary>
        /// Delete an entity. Please use DeactivateEntity unless required.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="entityName">Crm logical name.</param>
        /// <param name="entityId">Crm entity's id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeleteEntity(string entityName, Guid entityId)
        {
            EnlistInTransaction();
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceContext.DeleteEntityAsync(entityName, entityId), context);
        }

        /// <summary>
        /// Deactivate an entity.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="entityName">Crm logical name.</param>
        /// <param name="entityId">Crm entity's id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task DeactivateEntity(string entityName, Guid entityId)
        {
            EnlistInTransaction();
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.DeactivateEntityAsync(entityName, entityId), context);
        }

        /// <summary>
        /// Activate an entity.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="entityName">Crm logical name.</param>
        /// <param name="entityId">Crm entity's id.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ActivateEntity(string entityName, Guid entityId)
        {
            EnlistInTransaction();
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.ActivateEntityAsync(entityName, entityId), context);
        }

        /// <summary>
        /// This method is to be used for updating N:N relationships.
        /// Please note that this method is NOT TESTED yet.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="entity1Name">First entity name</param>
        /// <param name="entity1Id">First entity id</param>
        /// <param name="entity2Name">Second entity name</param>
        /// <param name="entity2Id">Second entity id</param>
        /// <param name="relationshipName">The relationship name </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task AssociateEntities(string entity1Name, Guid entity1Id, string entity2Name, Guid entity2Id, string relationshipName)
        {
            EnlistInTransaction();
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.AssociateEntitiesAsync(entity1Name, entity1Id, entity2Name, entity2Id, relationshipName), context);
        }

        /// <summary>
        /// This method is used to break N:N relationships.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="entity1Name">First entity name</param>
        /// <param name="entity1Id">First entity id</param>
        /// <param name="entity2Name">Second entity name</param>
        /// <param name="entity2Id">Second entity id</param>
        /// <param name="relationshipName">The relationship name </param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task UnAssociateEntities(string entity1Name, Guid entity1Id, string entity2Name, Guid entity2Id, string relationshipName)
        {
            EnlistInTransaction();
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.UnAssociateEntitiesAsync(entity1Name, entity1Id, entity2Name, entity2Id, relationshipName), context);
        }

        /// <summary>
        /// Fetch entities using xml.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="fetchXml">FetchXml used to query for entities.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<EntityCollection> FetchEntities(string fetchXml)
        {
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.FetchEntitiesAsync(fetchXml), context);
        }

        /// <summary>
        /// Activates the email entity.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="emailId">Crm email's id.</param>
        ///
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task ActivateEmailEntity(Guid emailId)
        {
            EnlistInTransaction();
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.ActivateEmailEntityAsync(emailId), context);
        }

        /// <summary>
        /// Retrieves the multiple entities.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="query">The query to retrieve multiple entities</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<EntityCollection> RetrieveMultipleEntities(QueryBase query)
        {
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.RetrieveMultipleEntitiesAsync(query), context);
        }

        /// <summary>
        /// Retrieves the entity.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<Entity> RetrieveEntity(string entityName, Guid entityId)
        {
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.RetrieveEntityAsync(entityName, entityId), context);
        }

        /// <summary>
        /// Retrieves the entity.
        /// </summary>
        /// <param name="context">Crm context in which preform the operation.</param>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="entityId">The entity identifier.</param>
        /// <param name="columns">The columns.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task<Entity> RetrieveEntity(string entityName, Guid entityId, ColumnSet columns)
        {
            var context = new Context().WithLogger(_logger);
            return _retryPolicy.ExecuteAsync(_ => ServiceProxy.RetrieveEntityAsync(entityName, entityId, columns), context);
        }

        public void Commit()
        {
            // Nothing to do here!
        }

        public void Rollback()
        {
            throw new RollbackException("Unable to rollback");
        }

        public void Rollback(Exception exc)
        {
            throw new RollbackException("Unable to rollback", exc);
        }

        public void SetTransactionManager(TransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        #region Constructor
        public CrmProvider(string connectionString, string wcfServiceUrl, ILogger logger)
        {
            _connectionString = connectionString;
            _wcfServiceUrl = wcfServiceUrl;
            _logger = logger;

            InitInnerContext();
            InitServiceProxy();
        }

        private void InitInnerContext()
        {
            var connection = new CrmConnection(new System.Configuration.ConnectionStringSettings("CRM", _connectionString)) { ServiceConfigurationInstanceMode = ServiceConfigurationInstanceMode.PerRequest };
            var organizationService = new OrganizationService(connection);
            ServiceContext = new DomesticCrmServiceContext(organizationService) { MergeOption = MergeOption.OverwriteChanges };
        }

        private void InitServiceProxy()
        {
            if (ServiceProxy != null)
            {
                return;
            }

            var clientCredentials = new ClientCredentials();

            // see if impersonation is used:
            if (_connectionString.Contains("DOMAIN") && _connectionString.Contains("Username") && _connectionString.Contains("Password"))
            {
                var keyValuePairs = _connectionString.Split(';')
                    .Select(value => value.Split('='))
                    .ToDictionary(pair => pair[0], pair => pair[1]);

                clientCredentials.Windows.ClientCredential.Domain = keyValuePairs["DOMAIN"];
                clientCredentials.Windows.ClientCredential.UserName = keyValuePairs["Username"];
                clientCredentials.Windows.ClientCredential.Password = keyValuePairs["Password"];
            }
            else
            {
                clientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
            }

            var organizationUri = new Uri(_wcfServiceUrl);
            ServiceProxy = new OrganizationServiceProxy(organizationUri, null, clientCredentials, null);
            ServiceProxy.EnableProxyTypes();
            ServiceProxy.Timeout = new TimeSpan(0, 10, 0);

            var binding = ServiceProxy.ServiceConfiguration.CurrentServiceEndpoint.Binding as CustomBinding;
            if (binding == null)
            {
                return;
            }

            foreach (var bindingElement in binding.Elements)
            {
                if (bindingElement is HttpTransportBindingElement httpTransportBindingElement)
                {
                    httpTransportBindingElement.UnsafeConnectionNtlmAuthentication = true;
                }
            }
        }
        #endregion Constructor

        #region IDisposable Support
        // This code added to correctly implement the disposable pattern.
#pragma warning disable SA1202 // Elements must be ordered by access
        public void Dispose()
#pragma warning restore SA1202 // Elements must be ordered by access
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (disposing)
            {
                ServiceContext?.Dispose();
                ServiceProxy?.Dispose();
            }

            ServiceContext = null;
            ServiceProxy = null;

            _disposedValue = true;
        }
        #endregion IDisposable Support

        private void EnlistInTransaction()
        {
            _transactionManager?.Current?.Enlist(this);
        }
    }
}
