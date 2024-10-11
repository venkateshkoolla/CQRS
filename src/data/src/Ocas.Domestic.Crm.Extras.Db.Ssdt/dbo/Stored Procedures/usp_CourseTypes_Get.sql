CREATE PROCEDURE [dbo].[usp_CourseTypes_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_coursetypeId AS Id
    , ocaslr_code AS Code
    , ocaslr_name AS [Name]
    , CASE @Locale
        WHEN 0 THEN ocaslr_englishdescription
        WHEN 1 THEN ocaslr_frenchdescription
        ELSE NULL
    END AS [LocalizedName]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_coursetypeBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
      (@Id IS NULL OR ocaslr_coursetypeId = @Id)
ORDER BY [Ocaslr_defaultvalue] DESC, [ocaslr_sortorder], [LocalizedName]