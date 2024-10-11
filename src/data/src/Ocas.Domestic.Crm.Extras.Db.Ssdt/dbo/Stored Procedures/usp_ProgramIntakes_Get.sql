CREATE PROCEDURE [dbo].[usp_ProgramIntakes_Get]
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @ApplicationCycleId UNIQUEIDENTIFIER = NULL,
	@CollegeId UNIQUEIDENTIFIER = NULL,
	@CampusId  UNIQUEIDENTIFIER = NULL,
	@ProgramCode NVARCHAR(8) = NULL,
	@ProgramTitle NVARCHAR(500) = NULL,
	@FromDate NVARCHAR(4) = NULL,
	@ProgramDeliveryId UNIQUEIDENTIFIER = NULL,
    @ProgramId UNIQUEIDENTIFIER = NULL,
    @Ids udtt_UniqueIdentifierList READONLY
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_ProgramIntakes] WHERE 1=1 ';

IF (@ApplicationCycleId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [ApplicationCycleId] = @ApplicationCycleId';

IF (@CollegeId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [CollegeId] = @CollegeId';

IF (@CampusId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [CampusId] = @CampusId';

IF (NULLIF(@ProgramCode,'') IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [ProgramCode] =  @ProgramCode';

IF (NULLIF(@ProgramTitle, '') IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [ProgramTitle] LIKE @ProgramTitle';

IF (NULLIF(@FromDate, '') IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [StartDate] = @FromDate';

IF (@ProgramDeliveryId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [ProgramDeliveryId] = @ProgramDeliveryId';

IF (@ProgramId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [ProgramId] = @ProgramId';

IF EXISTS(SELECT 1 from @Ids)
    SET @sqlCommand = @sqlCommand + ' AND [Id] IN (SELECT Id FROM @Ids)';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [StateCode] = @StateCode';

IF (@StatusCode IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [StatusCode] = @StatusCode';

SET @sqlCommand = @sqlCommand + ' ORDER BY [StartDate]'

SET @paramListTerms = '@ApplicationCycleId UNIQUEIDENTIFIER, @CollegeId UNIQUEIDENTIFIER, @CampusId UNIQUEIDENTIFIER, @ProgramCode NVARCHAR(8)
                      ,@ProgramTitle NVARCHAR(500), @FromDate NVARCHAR(4), @ProgramDeliveryId UNIQUEIDENTIFIER, @ProgramId UNIQUEIDENTIFIER, @StateCode BIT, @StatusCode TINYINT
                      ,@Ids udtt_UniqueIdentifierList READONLY';

    
EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @ApplicationCycleId = @ApplicationCycleId    
    , @CollegeId = @CollegeId
    , @CampusId = @CampusId
    , @ProgramCode = @ProgramCode
    , @ProgramTitle = @ProgramTitle
    , @FromDate = @FromDate
    , @ProgramDeliveryId = @ProgramDeliveryId
    , @ProgramId = @ProgramId
    , @Ids = @Ids
    , @StateCode = @StateCode
    , @StatusCode = @StatusCode
