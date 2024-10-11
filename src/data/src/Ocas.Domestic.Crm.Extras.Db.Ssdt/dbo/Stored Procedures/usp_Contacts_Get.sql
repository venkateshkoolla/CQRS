CREATE PROCEDURE [dbo].[usp_Contacts_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @ContactType INT = NULL,
    @UserName VARCHAR(100) = NULL,
    @SubjectId VARCHAR(100) = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_Contacts] WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Id] = @Id';

IF (@ContactType IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [ContactType] = @ContactType';

IF (@UserName IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Username] = @UserName';

IF (@SubjectId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [SubjectId] = @SubjectId';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [statecode] = @StateCode'

IF (@StatusCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [statuscode] = @StatusCode'

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ContactType INT, @UserName VARCHAR(100), @SubjectId VARCHAR(100), @StatusCode TINYINT, @StateCode BIT';
    
EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @Id = @Id
    , @ContactType = @ContactType
    , @UserName = @UserName
    , @SubjectId = @SubjectId
    , @StateCode = @StateCode
    , @StatusCode = @StatusCode
