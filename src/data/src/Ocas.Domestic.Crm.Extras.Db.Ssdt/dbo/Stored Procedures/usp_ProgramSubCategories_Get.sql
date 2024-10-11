CREATE PROCEDURE [dbo].[usp_ProgramSubCategories_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0,
    @ProgramCategoryId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_programsubcategoryId AS Id
    , ocaslr_code AS Code
    , ocaslr_name AS [Name]
    , CASE @Locale
        WHEN 0 THEN ocaslr_englishdescription
        WHEN 1 THEN ocaslr_frenchdescription
        ELSE NULL
    END AS [LocalizedName]
    , ocaslr_categoryid AS ProgramCategoryId
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programsubcategoryBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR ocaslr_programsubcategoryId = @Id) AND
    (@ProgramCategoryId IS NULL OR ocaslr_categoryid = @ProgramCategoryId)
ORDER BY [ocaslr_sortorder], [LocalizedName]
