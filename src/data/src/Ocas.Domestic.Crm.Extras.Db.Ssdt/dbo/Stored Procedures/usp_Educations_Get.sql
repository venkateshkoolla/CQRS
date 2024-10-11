CREATE PROCEDURE [dbo].[usp_Educations_Get]
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

    SET @sqlCommand = 'SELECT *
                       FROM [dbo].[view_Educations] [Educations]
                       WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Educations].Id = @Id';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Educations].[ApplicantId] = @ApplicantId';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Educations].[StateCode] = @StateCode';

IF (@StatusCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Educations].[StatusCode] = @StatusCode';

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @StateCode BIT, @StatusCode TINYINT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@ApplicantId = @ApplicantId
    ,@StateCode = @StateCode
    ,@StatusCode = @StatusCode
