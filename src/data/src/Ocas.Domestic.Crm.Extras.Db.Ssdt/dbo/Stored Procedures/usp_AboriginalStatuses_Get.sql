CREATE PROCEDURE [dbo].[usp_AboriginalStatuses_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_aboriginalstatusId AS Id
    , ocaslr_code AS Code
    , ocaslr_coltranecode AS ColtraneCode
    , ocaslr_name AS [Name]
    , CASE @Locale
        WHEN 0 THEN ocaslr_englishdescription
        WHEN 1 THEN ocaslr_frenchdescription
        ELSE NULL
    END AS [LocalizedName]
    , ocaslr_showinportal AS [ShowInPortal]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_aboriginalstatusBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR ocaslr_aboriginalstatusId = @Id)
ORDER BY [ocaslr_sortorder], [LocalizedName]
