CREATE PROCEDURE [dbo].[usp_InternationalCreditAssessments_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @StateCode INT = 0,
    @StatusCode TINYINT = 1
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [ocaslr_internationalcreditassessmentId] AS [Id]
      ,[ocaslr_applicantid] AS [ApplicantId]
      ,[ocaslr_internationalcreditassesmentstaid] AS [InternationalCreditAssessmentStatusId]
      ,[ocaslr_havecoursesecondaryevaluation] AS [HaveHighSchoolCourseEvaluation]
      ,[ocaslr_referencenumber] AS [ReferenceNumber]
      ,[ocaslr_coursesecondaryevaluatorid] AS [CredentialEvaluationAgencyId]
      ,[ocaslr_havepostsecondaryevaluation] AS [HavePostSecondaryEvaluation]
      , [CreatedOn]
      , [ModifiedOn]
      ,[ocaslr_modifiedbyuser] AS [ModifiedBy]
  FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_internationalcreditassessmentBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@StatusCode IS NULL OR statuscode = @StatusCode) AND
    (@ApplicantId IS NULL OR ocaslr_applicantid = @ApplicantId) AND
    (@Id IS NULL OR ocaslr_internationalcreditassessmentId = @Id)
