CREATE PROCEDURE [dbo].[usp_Programs_Get]
    @StateCode BIT = 0,
    @ApplicationCycleId UNIQUEIDENTIFIER = NULL,    
    @CollegeId UNIQUEIDENTIFIER = NULL,
    @Id UNIQUEIDENTIFIER = NULL,
    @CampusId UNIQUEIDENTIFIER = NULL,
    @DeliveryId UNIQUEIDENTIFIER = NULL,
    @Code NVARCHAR(8) = NULL,    
    @Title NVARCHAR(200) = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand NVARCHAR(MAX)
       , @paramListTerms NVARCHAR(MAX);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_Programs] WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND Id = @Id';

IF (@ApplicationCycleId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND ApplicationCycleId = @ApplicationCycleId';

IF (@CollegeId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND Collegeid = @CollegeId';

IF (@CampusId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND CampusId = @CampusId';

IF (@DeliveryId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND DeliveryId = @DeliveryId';

IF (NULLIF(@Code, '') IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND Code = @Code';

IF (NULLIF(@Title, '') IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND Title LIKE @Title';


IF (@StateCode IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [StateCode] = @StateCode';

    SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ApplicationCycleId UNIQUEIDENTIFIER, @CollegeId UNIQUEIDENTIFIER, @CampusId UNIQUEIDENTIFIER
                      ,@DeliveryId UNIQUEIDENTIFIER, @Code NVARCHAR(8), @Title NVARCHAR(200), @StateCode BIT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @Id = @Id
    , @ApplicationCycleId = @ApplicationCycleId
    , @CollegeId = @CollegeId
    , @CampusId = @CampusId
    , @DeliveryId = @DeliveryId
    , @Code = @Code
    , @Title = @Title
    , @StateCode = @StateCode