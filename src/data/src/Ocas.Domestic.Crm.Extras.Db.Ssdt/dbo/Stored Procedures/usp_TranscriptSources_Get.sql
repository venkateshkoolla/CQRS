﻿CREATE PROCEDURE [dbo].[usp_TranscriptSources_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_transcriptsourceId AS Id
    , ocaslr_Code AS Code
    , ocaslr_name AS [Name]
    , CASE @Locale
        WHEN 0 THEN ocaslr_EnglishDescription
        WHEN 1 THEN ocaslr_FrenchDescription
        ELSE NULL
    END AS [LocalizedName]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_transcriptsourceBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR ocaslr_transcriptsourceId = @Id)
ORDER BY ocaslr_SortOrder, [LocalizedName]