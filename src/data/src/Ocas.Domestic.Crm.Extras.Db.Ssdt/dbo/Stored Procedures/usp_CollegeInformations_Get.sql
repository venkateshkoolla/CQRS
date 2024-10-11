CREATE PROCEDURE [dbo].[usp_CollegeInformations_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @CollegeId UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

-- Refer here for more info
-- https://dev.azure.com/ocas/OCAS%20Portfolio/Codebusters/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Services%2FCollegeInformation%2FCollegeInformationService.cs&version=GBmaster&line=42&lineStyle=plain&lineEnd=73&lineStartColumn=9&lineEndColumn=10
-- https://dev.azure.com/ocas/OCAS%20Portfolio/Codebusters/_git/applicantportal?path=%2Fsrc%2FOCAS.ApplicantPortal.Web%2FControllers%2FCollegeInformationController.cs&version=GBmaster&line=106&lineStyle=plain&lineEnd=153&lineStartColumn=9&lineEndColumn=10

SELECT ocaslr_collegeinformationId AS [Id]
    , AccountId AS [CollegeId]
    , college.WebSiteURL AS [CollegeWebsiteURL]
    , CASE @Locale
        WHEN 0 THEN Ocaslr_englishwelcometext
        WHEN 1 THEN Ocaslr_frenchwelcometext
        ELSE NULL
      END AS [LocalizedWelcomeText]
    , CASE WHEN LEN(ocaslr_collegeinformationBase.ocaslr_phoneforrevoked) >= 3
        THEN LEFT(ocaslr_phoneforrevoked, 3)
        ELSE NULL
      END AS [RevokedOfferPhoneAreaCode]
    , CASE WHEN LEN(ocaslr_collegeinformationBase.ocaslr_phoneforrevoked) >= 7
        THEN SUBSTRING(ocaslr_phoneforrevoked, 3, 7)
        ELSE NULL
      END AS [RevokedOfferPhoneNumber]
    , ocaslr_revokedinformationurl AS [RevokedInformationUrl]
    , ocaslr_suspendedinformationurl AS [SuspendedInformationUrl]
    , ocaslr_auditreportemail AS [AuditReportEmail]
    , ocaslr_logo AS [FullLogoPath]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_collegeinformationBase]
     INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] AS college ON ocaslr_collegeid = AccountId
WHERE (@StateCode IS NULL OR ocaslr_collegeinformationBase.statecode = @StateCode) AND
    (@CollegeId IS NULL OR ocaslr_collegeinformationBase.ocaslr_collegeid = @CollegeId) AND
    (@Id IS NULL OR ocaslr_collegeinformationId = @Id)
