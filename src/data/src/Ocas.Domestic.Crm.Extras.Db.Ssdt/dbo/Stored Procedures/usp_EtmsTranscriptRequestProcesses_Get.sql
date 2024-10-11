CREATE PROCEDURE [dbo].[usp_EtmsTranscriptRequestProcesses_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @EtmsTranscriptRequestId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [Ocaslr_etmstranscriptrequestprocessId] AS [Id]
     , [Ocaslr_TranscriptRequestId] AS [EtmsTranscriptRequestid]
     , [Ocaslr_ProcessStartDate] AS [ProcessStartDate]
     , [Ocaslr_TranscriptRequestStatusId] AS [TranscriptRequestStatusId]
     , [Ocaslr_ProcessType] AS [EtmsProcessType]
     , [CreatedOn] AS [CreatedOn]
     , [ModifiedOn] AS [ModifiedOn]
     , [Ocaslr_LastModifiedBy] AS [ModifiedBy]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_etmstranscriptrequestprocessBase]
WHERE (@Id IS NULL OR [Ocaslr_etmstranscriptrequestprocessId] = @Id) AND
    (@EtmsTranscriptRequestId IS NULL OR [Ocaslr_TranscriptRequestId] = @EtmsTranscriptRequestId)
