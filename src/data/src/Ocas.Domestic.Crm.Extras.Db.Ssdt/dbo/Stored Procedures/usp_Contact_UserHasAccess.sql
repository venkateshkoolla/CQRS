CREATE PROCEDURE [dbo].[usp_Contact_UserHasAccess]
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @PartnerId NVARCHAR(50) = NULL,
    @UserType INT = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

IF(@UserType = 1) -- HighSchool user
 IF EXISTS (SELECT 1 FROM [dbo].[view_Applicants_HighSchool]
    WHERE [Mident] = @PartnerId  AND
          [ApplicantId] =  @ApplicantId)
    SELECT 1
 ELSE
    SELECT 0

ELSE IF(@UserType = 4) -- HighSchoolBoardUser
 IF EXISTS (SELECT 1 FROM [dbo].[view_Applicants_HighSchool]
    WHERE [BoardMident] = @PartnerId  AND
          [ApplicantId] =  @ApplicantId)
    SELECT 1
 ELSE
    SELECT 0

ELSE IF(@UserType = 2) -- College user
 IF EXISTS (SELECT 1 FROM [dbo].[view_Applicants_College]
    WHERE [CollegeCode] = @PartnerId  AND
          [ApplicantId] =  @ApplicantId)
    SELECT 1
 ELSE
    SELECT 0

ELSE SELECT 0