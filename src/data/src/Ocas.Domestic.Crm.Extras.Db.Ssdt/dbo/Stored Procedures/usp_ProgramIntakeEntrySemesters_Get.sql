CREATE PROCEDURE [dbo].[usp_ProgramIntakeEntrySemesters_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @EntrySemesterId UNIQUEIDENTIFIER = NULL,
    @ProgramIntakeId UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [ocaslr_programintakeentrysemesterId] AS Id
    , ocaslr_name AS [Name]
    , ocaslr_entrysemester AS [EntrySemesterId]
    , ocaslr_programintake AS [ProgramIntakeId]
    , CreatedOn
    , ModifiedOn
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programintakeentrysemesterBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@StatusCode IS NULL OR statuscode = @StatusCode) AND
    (@Id IS NULL OR ocaslr_programintakeentrysemesterId = @Id) AND
    (@EntrySemesterId IS NULL OR ocaslr_entrysemester = @EntrySemesterId) AND
    (@ProgramIntakeId IS NULL OR ocaslr_programintake = @ProgramIntakeId)
