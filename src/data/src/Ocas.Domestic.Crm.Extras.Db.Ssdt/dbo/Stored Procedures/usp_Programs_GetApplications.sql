CREATE PROCEDURE [dbo].[usp_Programs_GetApplications]
    @StateCode BIT = 0,
    @ProgramId UNIQUEIDENTIFIER = NULL,
    @IntakeId UNIQUEIDENTIFIER = NULL,
    @ApplicationStatusId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand NVARCHAR(MAX)
       , @paramListTerms NVARCHAR(MAX);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_ProgramApplications] WHERE 1=1';

IF (@ApplicationStatusId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND ApplicationStatusId = @ApplicationStatusId';

IF (@IntakeId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND IntakeId = @IntakeId';

IF (@ProgramId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND ProgramId = @ProgramId';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND StateCode = @StateCode';


SET @paramListTerms = '@ProgramId UNIQUEIDENTIFIER, @IntakeId UNIQUEIDENTIFIER, @ApplicationStatusId UNIQUEIDENTIFIER, @StateCode BIT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @ProgramId = @ProgramId
    , @IntakeId = @IntakeId
    , @ApplicationStatusId =  @ApplicationStatusId
    , @StateCode = @StateCode