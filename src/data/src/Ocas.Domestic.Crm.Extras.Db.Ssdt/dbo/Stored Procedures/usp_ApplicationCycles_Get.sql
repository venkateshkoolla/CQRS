CREATE PROCEDURE [dbo].[usp_ApplicationCycles_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_applicationcycleId AS Id
    ,ocaslr_year AS [Year]
    ,ACY.ocaslr_name AS [Name]
    ,ocaslr_startdate AS StartDate
    ,ocaslr_enddate AS EndDate
    ,ACY.ocaslr_applicationcyclestatusid AS StatusId
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcycleBase] AS ACY
WHERE (@StateCode IS NULL OR ACY.statecode = @StateCode) AND
      (@Id IS NULL OR ocaslr_applicationcycleId = @Id)
ORDER BY ocaslr_year

