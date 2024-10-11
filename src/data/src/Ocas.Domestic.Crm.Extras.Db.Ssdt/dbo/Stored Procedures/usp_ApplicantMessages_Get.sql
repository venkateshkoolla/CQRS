CREATE PROCEDURE [dbo].[usp_ApplicantMessages_Get]
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0,
    @CreatedOn DATETIME = NULL
AS
    SELECT 
      [ocaslr_applicantmessageId] AS [Id]
      ,[ocaslr_read] AS [HasRead]
      ,[ocaslr_applicantid] AS [ApplicantId]
    , CASE @Locale
        WHEN 0 THEN [ocaslr_englishsubject]
        WHEN 1 THEN [ocaslr_frenchsubject]
        ELSE NULL
    END AS [LocalizedSubject]
    , CASE @Locale
        WHEN 0 THEN [Ocaslr_englishtext]
        WHEN 1 THEN [Ocaslr_frenchtext]
        ELSE NULL
    END AS [LocalizedText]
      ,[CreatedOn]
  FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicantmessageBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@StatusCode IS NULL OR statuscode = @StatusCode) AND
    (@Id IS NULL OR [ocaslr_applicantmessageId] = @Id) AND
    (@ApplicantId IS NULL OR [ocaslr_applicantid] = @ApplicantId) AND
    (@CreatedOn IS NULL OR [CreatedOn] >= @CreatedOn)
ORDER BY [CreatedOn] DESC, [HasRead]
