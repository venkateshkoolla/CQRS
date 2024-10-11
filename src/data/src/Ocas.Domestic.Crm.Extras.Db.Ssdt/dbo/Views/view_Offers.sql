﻿CREATE VIEW [dbo].[view_Offers]
    AS
SELECT [Offer].[ocaslr_offerId] AS [Id]
    , [Application].[ocaslr_applicantid] AS [ApplicantId]
    , [Application].[ocaslr_applicationId] AS [ApplicationId]
    , [Application].[ocaslr_applicationnumber] AS [ApplicationNumber]
    , [Application].[ocaslr_applicationstatusid] AS [ApplicationStatusId]
    , [Application].[ocaslr_applicationcycleid] AS [ApplicationCycleId]
    , [Offer].[ocaslr_hardexpirydate] AS [HardExpiryDate]
    , [Offer].[ocaslr_softexpirydate] AS [SoftExpiryDate]
    , [Offer].[ocaslr_startdate] AS [StartDate]
    , [Offer].[Ocaslr_sequencenumber] AS [SequenceNumber]
    , [Offer].[ocaslr_termidentifier] AS [TermIdentifier]
    , [Offer].[ocaslr_lateadmit] AS [IsLateAdmit]
    , [Offer].[ocaslr_effectivedate] AS [PostedDate]
    , [Offer].[ocaslr_lock] AS [OfferLockReleaseDate]
    , [Offer].[ocaslr_confirmeddate] AS [ConfirmedDate]
    , [Offer].[ocaslr_offerstateid] AS [OfferStateId]
    , [Offer].[ocaslr_offerstatusid] AS [OfferStatusId]
    , [Offer].[ocaslr_fullparttimeid] AS [OfferStudyMethodId]
    , [Offer].[ocaslr_offertypeid] AS [OfferTypeId]
    , [Offer].[ocaslr_entrylevelid] AS [EntryLevelId]
    , [Offer].[ocaslr_programintakeid] AS [IntakeId]
    , [Program].[ocaslr_programId] AS [ProgramId]
    , [Program].[ocaslr_programcode] AS [ProgramCode]
    , [Program].[ocaslr_name] AS [ProgramName]
    , [Program].[ocaslr_campusid] AS [CampusId]
    , CASE 
        WHEN [Campus].[ocaslr_brandname] IS NOT NULL AND [Campus].[ocaslr_brandname] <> '' THEN [Campus].[ocaslr_brandname]
        ELSE [Campus].[Name]
      END AS [CampusName]
    , [CollegeApplicationCycle].[ocaslr_collegeid] AS [CollegeId]
    , [CollegeApplicationCycle].[ocaslr_collegeapplicationcycleId] AS [CollegeApplicationCycleId]
    , [Offer].[CreatedOn]
    , [Offer].[ModifiedOn]
    , [Offer].[ocaslr_modifiedbyuser] AS [ModifiedBy]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].ocaslr_offerBase [Offer]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationBase] [Application]
        ON [Offer].[ocaslr_applicationid] = [Application].[ocaslr_applicationId]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programintakeBase] [ProgramIntake]
        ON [ProgramIntake].[ocaslr_programintakeId] = [Offer].[ocaslr_programintakeid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programBase] [Program]
        ON [ProgramIntake].[ocaslr_programid] = [Program].[ocaslr_programId]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_offerstateBase] [OfferState]
        ON [OfferState].[ocaslr_offerstateId] = [Offer].[ocaslr_offerstateid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_offerstatusBase] [OfferStatus]
        ON [OfferStatus].[ocaslr_offerstatusId] = [Offer].[ocaslr_offerstatusid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_offertypeBase] [OfferType]
        ON [OfferType].[Ocaslr_offertypeId] = [Offer].[ocaslr_offertypeid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_offerstudymethodBase] [OfferStudyMethod]
        ON [OfferStudyMethod].[ocaslr_offerstudymethodId] = [Offer].[ocaslr_fullparttimeid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_collegeapplicationcycleBase] [CollegeApplicationCycle]
        ON [CollegeApplicationCycle].[ocaslr_collegeapplicationcycleId] = [Program].[ocaslr_collegeapplicationcycleid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcycleBase] [ApplicationCycle]
        ON [Application].[ocaslr_applicationcycleid] = [ApplicationCycle].[ocaslr_applicationcycleId]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcyclestatusBase] [ApplicationCycleStatus]
        ON [ApplicationCycleStatus].[ocaslr_applicationcyclestatusId] = [ApplicationCycle].[ocaslr_applicationcyclestatusid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] [Campus]
        ON [Campus].[AccountId] = [Program].[ocaslr_campusid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ContactBase] [Applicant]
        ON [Applicant].[ContactId] = [Application].[ocaslr_applicantid]
WHERE
    [Application].[statecode] = 0 AND
    [Offer].[statecode] = 0 AND
    CAST([Offer].[ocaslr_effectivedate] AS date) <= CAST(GETDATE() AS date) AND
    [ApplicationCycleStatus].[ocaslr_code] = 'A' AND
    [OfferState].ocaslr_code <> 'D'
