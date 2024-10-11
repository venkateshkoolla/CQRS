CREATE PROCEDURE [dbo].[usp_TranscriptTransmissions_Get]
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Locale INT = 0,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_transcripttransmissionId AS Id
    , ocaslr_term AS Code
    , ocaslr_name AS [Name]
    , CASE @Locale
        WHEN 0 THEN ocaslr_englishdescription
        WHEN 1 THEN ocaslr_frenchdescription
        ELSE NULL
    END AS [LocalizedName]
    , ocaslr_institutiontype AS InstitutionType
    , ocaslr_termduedate AS TermDueDate
    , ocaslr_termenddate AS TermEndDate
    , ocaslr_termstartdate AS TermStartDate
    , ocaslr_applicationcycleid AS ApplicationCycleId
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_transcripttransmissionBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@StatusCode IS NULL OR statuscode = @StatusCode) AND
    (@Id IS NULL OR ocaslr_transcripttransmissionId = @Id)
ORDER BY ocaslr_institutiontype, ocaslr_termstartdate
