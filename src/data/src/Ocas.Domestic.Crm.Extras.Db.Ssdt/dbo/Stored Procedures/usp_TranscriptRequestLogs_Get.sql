CREATE PROCEDURE [dbo].[usp_TranscriptRequestLogs_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
      
SELECT [ocaslr_peterequestlogId] AS Id
    ,[Ocaslr_servicerequest] As ServiceRequest
    ,[ocaslr_serviceresponsecode] AS ServiceResponseCode
    ,[ocaslr_orderid] AS OrderId
    ,[ocaslr_requesttimestamp] AS RequestTimestamp
    ,[Ocaslr_serviceresponse] AS ServiceResponse
    ,[Ocaslr_processedstatus] AS ProcessStatus
    ,[ocaslr_transcriptrequeststatusid] AS TranscriptRequestStatusId
    ,[ocaslr_name] AS [Name] 
    ,[CreatedOn]
    ,[ModifiedOn]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_peterequestlogBase]
WHERE (@StateCode IS NULL OR [statecode] = @StateCode) AND
    (@StatusCode IS NULL OR [statuscode] = @StatusCode) AND
    (@Id IS NULL OR [ocaslr_peterequestlogId] = @Id)
