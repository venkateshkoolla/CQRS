CREATE PROCEDURE [dbo].[usp_OntarioStudentCourseCredits_Get]
    @TranscriptId UNIQUEIDENTIFIER = NULL,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_OntarioStudentCourseCredits] [Grades] WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Grades].Id = @Id';

IF (@TranscriptId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Grades].[TranscriptId] = @TranscriptId';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Grades].[ApplicantId] = @ApplicantId';

IF (@StateCode IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Grades].[StateCode] = @StateCode';

SET @sqlCommand = @sqlCommand + ' ORDER BY [Grades].[CompletedDate], [Grades].[CourseCode] DESC'

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @TranscriptId UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @StateCode BIT';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@TranscriptId = @TranscriptId
    ,@ApplicantId = @ApplicantId
    ,@StateCode = @StateCode
