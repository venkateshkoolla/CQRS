CREATE PROCEDURE [dbo].[usp_Offers_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @ApplicationStatusId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_Offers] WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Id] = @Id';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicantId] = @ApplicantId';

IF (@ApplicationId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicationId] = @ApplicationId';

IF (@ApplicationStatusId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicationStatusId] = @ApplicationStatusId';
     
SET @sqlCommand = @sqlCommand + ' ORDER BY [CreatedOn]';
SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @ApplicationId UNIQUEIDENTIFIER, @ApplicationStatusId UNIQUEIDENTIFIER';
    
EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @Id = @Id
    , @ApplicantId = @ApplicantId
    , @ApplicationId = @ApplicationId
    , @ApplicationStatusId = @ApplicationStatusId
