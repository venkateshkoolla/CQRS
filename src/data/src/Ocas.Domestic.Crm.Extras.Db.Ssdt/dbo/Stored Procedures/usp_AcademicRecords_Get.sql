CREATE PROCEDURE [dbo].[usp_AcademicRecords_Get]
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
                       FROM [dbo].[view_AcademicRecords] [AcademicRecords]
                       WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [AcademicRecords].[Id] = @Id';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [AcademicRecords].[ApplicantId] = @ApplicantId';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [AcademicRecords].[StateCode] = @StateCode';

IF (@StatusCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [AcademicRecords].[StatusCode] = @StatusCode';

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @StateCode BIT, @StatusCode TINYINT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@ApplicantId = @ApplicantId
    ,@StateCode = @StateCode
    ,@StatusCode = @StatusCode
