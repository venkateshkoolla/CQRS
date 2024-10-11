CREATE PROCEDURE [dbo].[usp_DocumentPrints_Get]
	@StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @CollegeId UNIQUEIDENTIFIER = NULL,
    @DocumentTypeId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [Ocaslr_documentprinttableId] AS [Id]
    , [ocaslr_collegeid] AS [CollegeId]
    , [ocaslr_SendToColtrane] AS [SendToColtrane]
    , [ocaslr_documenttypeid] AS [DocumentTypeId]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_documentprinttableBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR Ocaslr_documentprinttableId = @Id) AND
    (@CollegeId IS NULL OR ocaslr_collegeid = @CollegeId) AND
    (@DocumentTypeId IS NULL OR ocaslr_documenttypeid = @DocumentTypeId)