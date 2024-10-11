CREATE PROCEDURE [dbo].[usp_SupportingDocuments_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

    SET @sqlCommand = 'SELECT *
                       FROM [dbo].[view_SupportingDocuments] [SupportingDocuments]
                       WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [SupportingDocuments].Id = @Id';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [SupportingDocuments].[ApplicantId] = @ApplicantId';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [SupportingDocuments].[StateCode] = @StateCode';

IF (@StatusCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [SupportingDocuments].[StatusCode] = @StatusCode';

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @StateCode BIT, @StatusCode TINYINT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@ApplicantId = @ApplicantId
    ,@StateCode = @StateCode
    ,@StatusCode = @StatusCode
