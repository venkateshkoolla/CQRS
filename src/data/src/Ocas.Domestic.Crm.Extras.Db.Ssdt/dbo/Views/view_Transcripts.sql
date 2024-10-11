CREATE VIEW [dbo].[view_Transcripts]
	AS
SELECT [ocaslr_transcriptId] AS [Id]
      ,[ocaslr_originalstudentid] AS [OriginalStudentId]
      ,[ocaslr_transcripttype] AS [TranscriptType]
      ,[Transcript].[ocaslr_name] AS [Name]
      ,[ocaslr_partnerid] AS [PartnerId]
      ,[ocaslr_contactid] AS [ContactId]
      ,[Ocaslr_credentials] AS [Credentials]
      ,[ocaslr_eTMSTranscriptId] AS [eTMSTranscriptId]
      ,[ocaslr_SupportingDocumentId] AS [SupportingDocumentId]
      ,[ocaslr_TranscriptSourceId] AS [TranscriptSourceId]
      ,[Transcript].[CreatedOn] AS [CreatedOn]
      ,[Transcript].[ModifiedOn] AS [ModifiedOn]
      ,[ocaslr_modifiedbyuser] AS [ModifiedBy]
      ,[Transcript].[statecode] AS [StateCode]
      ,[Transcript].[statuscode] AS [StatusCode]
      ,[account_parent_account].[ocaslr_boardid] AS [BoardMident]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_transcriptBase] [Transcript]
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] ON [Transcript].ocaslr_partnerid = [AccountBase].AccountId
	LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] [account_parent_account] ON (AccountBase.ParentAccountId = [account_parent_account].AccountId)

