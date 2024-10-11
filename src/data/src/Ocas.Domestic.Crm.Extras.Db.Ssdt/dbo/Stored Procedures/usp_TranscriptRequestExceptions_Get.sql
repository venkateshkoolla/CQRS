CREATE PROCEDURE [dbo].[usp_TranscriptRequestExceptions_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT Ocaslr_transcriptrequestexceptionId AS [Id]
    , Ocaslr_name AS [Name]
    , CASE @Locale
        WHEN 0 THEN Ocaslr_EnglishDescription
        WHEN 1 THEN Ocaslr_FrenchDescription
        ELSE NULL
    END AS [LocalizedName]
    , Ocaslr_PartnerId AS [InstituteId]
    , Ocaslr_MessageLevel AS [ExceptionLevel]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_transcriptrequestexceptionBase]
WHERE (@StateCode IS NULL OR [statecode] = @StateCode) AND
    (@StatusCode IS NULL OR [statuscode] = @StatusCode) AND
    (@Id IS NULL OR ocaslr_transcriptrequestexceptionId = @Id)