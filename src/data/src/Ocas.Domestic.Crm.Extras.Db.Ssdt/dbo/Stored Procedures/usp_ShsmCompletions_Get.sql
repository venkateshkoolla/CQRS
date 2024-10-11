CREATE PROCEDURE [dbo].[usp_ShsmCompletions_Get]
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [ocaslr_shsmcompletionId] AS Id
      ,[ocaslr_code] AS Code
      ,[ocaslr_name] AS Name
      ,[ocaslr_description] AS LocalizedName
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_shsmcompletionBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
      (@StatusCode IS NULL OR statuscode = @StatusCode) AND
      (@Id IS NULL OR ocaslr_shsmcompletionId = @Id)
