CREATE PROCEDURE [dbo].[usp_Cities_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0,
    @ProvinceId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_cityId AS Id
    , ocaslr_code AS Code
    , ocaslr_name AS [Name]
    , CASE @Locale
        WHEN 0 THEN ocaslr_englishdescription
        WHEN 1 THEN ocaslr_frenchdescription
        ELSE NULL
    END AS [LocalizedName]
    , ocaslr_provincestateid [ProvinceId]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_cityBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR ocaslr_cityId = @Id) AND
    (@ProvinceId IS NULL OR ocaslr_provincestateid = @ProvinceId) AND
    ocaslr_provincestateid IS NOT NULL
ORDER BY [ocaslr_sortorder], [LocalizedName]
