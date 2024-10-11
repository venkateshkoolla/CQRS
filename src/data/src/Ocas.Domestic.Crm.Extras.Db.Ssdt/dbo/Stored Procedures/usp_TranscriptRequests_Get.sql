CREATE PROCEDURE [dbo].[usp_TranscriptRequests_Get]
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @EducationId UNIQUEIDENTIFIER = NULL,
    @Id UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_TranscriptRequests] WHERE 1=1 ';

IF (@ApplicationId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [ApplicationId] = @ApplicationId';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicantId] = @ApplicantId';

IF (@EducationId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [EducationId] = @EducationId';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Id] = @Id';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [statecode] = @StateCode'

IF (@StatusCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [statuscode] = @StatusCode'

SET @paramListTerms = '@ApplicationId UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @EducationId UNIQUEIDENTIFIER, @Id UNIQUEIDENTIFIER, @StateCode BIT, @StatusCode TINYINT';
    
EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @ApplicationId = @ApplicationId    
    , @ApplicantId = @ApplicantId
    , @EducationId = @EducationId
    , @Id = @Id
    , @StateCode = @StateCode
    , @StatusCode = @StatusCode
