CREATE PROCEDURE [dbo].[usp_Transcripts_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @ContactId UNIQUEIDENTIFIER = NULL,
    @PartnerId UNIQUEIDENTIFIER = NULL,
    @BoardMident NVARCHAR(10) = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

    SET @sqlCommand = 'SELECT *
                       FROM [dbo].[view_Transcripts] [Transcripts]
                       WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Transcripts].[Id] = @Id';

IF (@ContactId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Transcripts].[ContactId] = @ContactId';

IF (@PartnerId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Transcripts].[PartnerId] = @PartnerId';

IF (@BoardMident IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Transcripts].[BoardMident] = @BoardMident';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Transcripts].[StateCode] = @StateCode';

IF (@StatusCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Transcripts].[StatusCode] = @StatusCode';

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ContactId UNIQUEIDENTIFIER, @PartnerId UNIQUEIDENTIFIER, @BoardMident NVARCHAR(10), @StateCode INT, @StatusCode TINYINT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@ContactId = @ContactId
    ,@PartnerId = @PartnerId
    ,@StateCode = @StateCode
    ,@StatusCode = @StatusCode
    ,@BoardMident = @BoardMident