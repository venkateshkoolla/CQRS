CREATE PROCEDURE [dbo].[usp_PrivacyStatements_Get]
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0,
    @Category INT = NULL,
    @EffectiveDate DATETIME = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_privacystatementId AS Id
    , ocaslr_name AS [Name]
    , ocaslr_category AS [Category]
    , ocaslr_version AS [Version]
    , CASE @Locale
        WHEN 0 THEN Ocaslr_englishprivacystatement
        WHEN 1 THEN Ocaslr_frenchprivacystatement
        ELSE NULL
    END [Content]
    , ocaslr_effectivedate AS [EffectiveDate]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_privacystatementBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@StatusCode IS NULL OR statuscode = @StatusCode) AND
    (@Id IS NULL OR ocaslr_privacystatementId = @Id) AND
    (@Category IS NULL OR OCASLR_Category = @Category) AND
    (@EffectiveDate IS NULL OR ocaslr_effectivedate <= @EffectiveDate)
ORDER BY ocaslr_effectivedate DESC
