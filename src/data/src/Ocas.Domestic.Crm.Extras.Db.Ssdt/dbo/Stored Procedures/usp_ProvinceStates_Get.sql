CREATE PROCEDURE [dbo].[usp_ProvinceStates_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0,
    @CountryId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_provincestateId AS Id
    , ocaslr_code AS Code
    , ocaslr_name AS [Name]
    , ocaslr_countryid AS CountryId
    , CASE @Locale
        WHEN 0 THEN ocaslr_englishdescription
        WHEN 1 THEN ocaslr_frenchdescription
        ELSE NULL
    END AS [LocalizedName]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_provincestateBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR ocaslr_provincestateId = @Id) AND
    (@CountryId IS NULL OR ocaslr_countryid = @CountryId)
ORDER BY [ocaslr_sortorder], [LocalizedName]
