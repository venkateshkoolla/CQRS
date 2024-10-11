﻿CREATE PROCEDURE [dbo].[usp_OfferTypes_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT Ocaslr_offertypeId AS Id
    , Ocaslr_code AS Code
    , Ocaslr_name AS [Name]
    , CASE @Locale
        WHEN 0 THEN Ocaslr_englishdescription
        WHEN 1 THEN Ocaslr_FrenchDescription
        ELSE NULL
    END AS [LocalizedName]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_offertypeBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR Ocaslr_offertypeId = @Id)
ORDER BY [Ocaslr_sortorder], [LocalizedName]
