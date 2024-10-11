CREATE PROCEDURE [dbo].[usp_CollegeApplicationCycles_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @CollegeId UNIQUEIDENTIFIER = NULL,
    @ApplicationCycleId  UNIQUEIDENTIFIER = NULL
AS
SELECT ocaslr_collegeapplicationcycleId AS Id
    , ocaslr_name AS [Name]
    , ocaslr_collegeid AS CollegeId
    , ocaslr_applicationcycleid AS ApplicationCycleId
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_collegeapplicationcycleBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
      (@Id IS NULL OR ocaslr_collegeapplicationcycleId = @Id) AND 
      (@CollegeId IS NULL OR ocaslr_collegeid = @CollegeId) AND 
      (@ApplicationCycleId IS NULL OR ocaslr_applicationcycleid = @ApplicationCycleId) 
ORDER BY CollegeId, [Name]
