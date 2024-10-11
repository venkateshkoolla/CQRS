CREATE PROCEDURE [dbo].[usp_DocumentDistributionRecords_Get]
	@StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicationId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [Ocaslr_documentdistributionrecordId] AS [Id] 
    , [ocaslr_collegeid] as CollegeId
    , [ocaslr_supportingdocumentid] as SupportingDocumentId
    , [Ocaslr_name] as [Name]
    , [ocaslr_applicationid] as ApplicationId
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_documentdistributionrecordBase]
WHERE (@Id IS NULL OR [Ocaslr_documentdistributionrecordId] = @Id) AND
    (@ApplicationId IS NULL OR [ocaslr_applicationid] = @ApplicationId) AND
    (@StateCode IS NULL OR [statecode] = @StateCode) AND
    (@StatusCode IS NULL OR [statuscode] = @StatusCode)