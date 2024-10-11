CREATE PROCEDURE [dbo].[usp_ProgramChoices_Get]
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @Id UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_ProgramChoices] WHERE 1=1 ';

IF (@ApplicationId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [ApplicationId] = @ApplicationId';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [ApplicantId] = @ApplicantId';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Id] = @Id';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [statecode] = @StateCode'

SET @sqlCommand = @sqlCommand + ' ORDER BY [SequenceNumber]';
SET @paramListTerms = '@ApplicationId UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @Id UNIQUEIDENTIFIER, @StateCode BIT';

    
EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @ApplicationId = @ApplicationId    
    , @ApplicantId = @ApplicantId
    , @Id = @Id
    , @StateCode = @StateCode
