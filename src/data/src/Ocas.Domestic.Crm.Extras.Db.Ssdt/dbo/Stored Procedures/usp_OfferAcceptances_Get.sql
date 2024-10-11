CREATE PROCEDURE [dbo].[usp_OfferAcceptances_Get]
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0,
    @StatusCode BIT = 1,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT 
    oab.ocaslr_offeracceptanceId AS Id,
    oeb.ocaslr_applicantid AS ApplicantId,
    appb.ocaslr_applicationId AS ApplicationId,
    oab.ocaslr_accepteddate AS AcceptedDate, 
    oeb.ocaslr_entrylevelid AS EntryLevelId,
    oab.ocaslr_declineddate AS DeclinedDate,
    ae.ocaslr_brandname AS CollegeName,
    ae.ocaslr_collegecode AS CollegeId,
    peb.ocaslr_title AS ProgramTitle,
    peb.ocaslr_programcode AS ProgramCode,
    peb.ocaslr_programId AS ProgramId,
    peb.ocaslr_ProgramDelivery AS ProgramDeliveryId,
    ae2.ocaslr_campuscode AS CampusId,
    CASE 
        WHEN (ae2.ocaslr_brandname is null) THEN ae2.Name
        WHEN (ae2.ocaslr_brandname = '') THEN ae2.Name
        ELSE ae2.ocaslr_brandname
    END AS CampusName,
    pieb.Ocaslr_startdate AS StartDate,
    eleb.ocaslr_code AS EntryLevelCode,
    CASE @Locale
        WHEN 0 THEN steb.ocaslr_englishdescription
        WHEN 1 THEN steb.ocaslr_frenchdescription
    ELSE NULL
    END AS LocalizedProgramDelivery,
    oeb.ocaslr_offerstatusid AS OfferStatusId,
    CASE @Locale
        WHEN 0 THEN oseb2.ocaslr_englishdescription
        WHEN 1 THEN oseb2.ocaslr_frenchdescription
    ELSE NULL
    END AS LocalizedOfferStatusDescription,
    appb.ocaslr_applicationcycleid AS ApplicationCycleId,
    appb.ocaslr_applicationId AS ApplicationId,
    oab.CreatedOn,
    oab.ModifiedOn
FROM 
    [$(SERVER)].[OCAS_MSCRM].[dbo].ocaslr_offeracceptanceBase oab
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_offerBase] oeb
    ON oab.ocaslr_offerid=oeb.ocaslr_offerId
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationBase] appb
    ON oeb.ocaslr_applicationid = appb.ocaslr_applicationId
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programintakeBase] pieb
    ON oeb.ocaslr_programintakeid=pieb.ocaslr_programintakeId
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programBase] peb
    ON pieb.ocaslr_programid = peb.ocaslr_programId
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] ae2
    ON peb.ocaslr_campusid = ae2.AccountId 
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_collegeapplicationcycleBase] caeb
    ON peb.ocaslr_collegeapplicationcycleid = caeb.ocaslr_collegeapplicationcycleId
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcycleBase] acb
    ON acb.ocaslr_applicationcycleId = caeb.ocaslr_applicationcycleid
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcyclestatusBase] acsb
    ON acsb.ocaslr_applicationcyclestatusId = acb.ocaslr_applicationcyclestatusid
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] ae
    ON caeb.ocaslr_collegeid = ae.AccountId
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_offertypeBase] oteb
    ON oteb.Ocaslr_offertypeId = oeb.ocaslr_offertypeid
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_offerstateBase] oseb
    ON oseb.ocaslr_offerstateId = oeb.ocaslr_offerstateid
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_offerstatusBase] oseb2
    ON oseb2.ocaslr_offerstatusId = oeb.ocaslr_offerstatusid
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_entrylevelBase] eleb
    ON eleb.ocaslr_entrylevelId = oeb.ocaslr_entrylevelid
INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_offerstudymethodBase] steb
    ON steb.ocaslr_offerstudymethodId = peb.ocaslr_ProgramDelivery
WHERE 
    (@StateCode IS NULL OR oab.statecode = @StateCode) AND
    (@StatusCode IS NULL OR oab.statuscode = @StatusCode) AND
    (@Id IS NULL OR oab.ocaslr_offeracceptanceId = @Id) AND
    (@ApplicationId IS NULL OR appb.ocaslr_applicationId = @ApplicationId) AND
    CAST(oeb.ocaslr_effectivedate AS date) <= CAST(GETDATE() AS date) AND
    acsb.ocaslr_code = 'A' AND
    oseb.ocaslr_code <> 'D'
ORDER BY oeb.CreatedOn