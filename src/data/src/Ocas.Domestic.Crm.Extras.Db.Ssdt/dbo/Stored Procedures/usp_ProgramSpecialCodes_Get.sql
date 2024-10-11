CREATE PROCEDURE [dbo].[usp_ProgramSpecialCodes_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @CollegeApplicationId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_programspecialcodeId AS Id
    , ocaslr_code AS Code
    , ocaslr_name AS [Name]
    , ocaslr_description AS [LocalizedName]
    , ocaslr_collegeapplicatiocycleid AS CollegeApplicationId
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programspecialcodeBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR ocaslr_programspecialcodeId = @Id) AND
    (@CollegeApplicationId IS NULL OR ocaslr_collegeapplicatiocycleid = @CollegeApplicationId)
ORDER BY [ocaslr_sortorder], [LocalizedName]

