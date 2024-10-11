CREATE PROCEDURE [dbo].[usp_ProgramEntryLevels_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @EntryLevelId UNIQUEIDENTIFIER = NULL,
    @ProgramId UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [ocaslr_program_entrylevelsId] AS Id
    , ocaslr_name AS [Name]
    , ocaslr_entrylevelid AS [EntryLevelId]
    , ocaslr_programid AS [ProgramId]
    , CreatedOn
    , ModifiedOn
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_program_entrylevelsBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@StatusCode IS NULL OR statuscode = @StatusCode) AND
    (@Id IS NULL OR ocaslr_program_entrylevelsId = @Id) AND
    (@EntryLevelId IS NULL OR ocaslr_entrylevelid = @EntryLevelId) AND
    (@ProgramId IS NULL OR ocaslr_programid = @ProgramId)