CREATE PROCEDURE [dbo].[usp_OntarioHighSchoolCourseCodes_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @Name NVARCHAR(10) = NULL,
    @StateCode BIT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_ontariohighschoolcoursecodesId AS Id
    , ocaslr_name AS [Name]
    , ocaslr_title AS [Title]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_ontariohighschoolcoursecodesBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
      (@StatusCode IS NULL OR statuscode = @StatusCode) AND
      (@Id IS NULL OR ocaslr_ontariohighschoolcoursecodesId = @Id) AND
	  (@Name IS NULL OR ocaslr_name = @Name)