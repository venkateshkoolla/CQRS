CREATE PROCEDURE [dbo].[usp_Applications_Get]
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicationStatusId UNIQUEIDENTIFIER = NULL,
    @ApplicationNumber VARCHAR(12) = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @ApplicationCycleId UNIQUEIDENTIFIER = NULL,
    @ShoppingCartStatus INT = NULL,
    @ApplicationCycleStatus CHAR = 'A'
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_Applications] WHERE 1=1 ';

IF(@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [StateCode] = @StateCode'

IF(@StatusCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [StatusCode] = @StatusCode'

IF(@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Id] = @Id'

IF(@ApplicationStatusId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicationStatusId] = @ApplicationStatusId'

IF(@ApplicationNumber IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicationNumber] = @ApplicationNumber'

IF(@ApplicantId  IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicantId] = @ApplicantId'

IF(@ApplicationCycleId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicationCycleId] = @ApplicationCycleId'

IF(@ShoppingCartStatus IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ShoppingCartStatus] = @ShoppingCartStatus'

IF(@ApplicationCycleStatus IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Code] = @ApplicationCycleStatus'

    SET @sqlCommand = @sqlCommand + ' ORDER BY ModifiedOn DESC'

SET @paramListTerms = '@StateCode BIT, @StatusCode TINYINT, @Id UNIQUEIDENTIFIER, @ApplicationStatusId UNIQUEIDENTIFIER, @ApplicationNumber VARCHAR(12), @ApplicantId UNIQUEIDENTIFIER, @ApplicationCycleId UNIQUEIDENTIFIER, @ShoppingCartStatus INT, @ApplicationCycleStatus CHAR';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @Id = @Id
    , @ApplicantId = @ApplicantId
    , @StateCode = @StateCode
    , @StatusCode = @StatusCode
    , @ApplicationStatusId = @ApplicationStatusId
    , @ApplicationNumber = @ApplicationNumber
    , @ApplicationCycleId = @ApplicationCycleId
    , @ShoppingCartStatus = @ShoppingCartStatus
    , @ApplicationCycleStatus = @ApplicationCycleStatus

