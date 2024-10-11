CREATE PROCEDURE [dbo].[usp_Countries_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Name NVARCHAR(255) = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT
    ocaslr_countryId AS Id,
    ocaslr_code AS Code,
    ocaslr_name AS [Name],
    CASE @Locale
        WHEN 0 THEN ocaslr_englishdescription
        WHEN 1 THEN ocaslr_frenchdescription
        ELSE NULL
    END AS [LocalizedName]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_countryBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR ocaslr_countryId = @Id) AND
    (@Name IS NULL OR ocaslr_name = @Name)
ORDER BY [ocaslr_sortorder], [LocalizedName]
