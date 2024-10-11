CREATE PROCEDURE [dbo].[usp_Institutes_Get]
    @StateCode INT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [ocaslr_instituteId] AS Id
      ,[ocaslr_code] AS Code
      ,[ocaslr_name] AS [Name]
  FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_instituteBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
      (@StatusCode IS NULL OR statuscode = @StatusCode) AND
      (@Id IS NULL OR [ocaslr_instituteId] = @Id)
ORDER BY [ocaslr_name]
