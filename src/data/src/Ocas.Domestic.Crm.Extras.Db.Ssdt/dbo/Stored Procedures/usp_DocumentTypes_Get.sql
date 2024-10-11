CREATE PROCEDURE [dbo].[usp_DocumentTypes_Get]    
    @StateCode INT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_documentypeId AS Id
      ,ocaslr_code AS Code
      ,ocaslr_name AS [Name]
      ,CASE @Locale
          WHEN 0 THEN ocaslr_englishdescription
          WHEN 1 THEN ocaslr_frenchdescription
          ELSE NULL
      END AS [LocalizedName]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_documentypeBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
      (@StatusCode IS NULL OR statuscode = @StatusCode) AND
      (@Id IS NULL OR [ocaslr_documentypeId] = @Id)
ORDER BY [ocaslr_sortorder], [LocalizedName]
