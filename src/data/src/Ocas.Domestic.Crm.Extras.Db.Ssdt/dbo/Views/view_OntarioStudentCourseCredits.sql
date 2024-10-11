CREATE VIEW [dbo].[view_OntarioStudentCourseCredits]
    AS
SELECT ocaslr_ontariostudentcoursecreditId AS [Id],
       ocaslr_contactid AS [ApplicantId],
       ocaslr_coursecode AS [CourseCode],
       ocaslr_completiondate AS [CompletedDate],
       ocaslr_note AS [Notes],
       ocaslr_coursemident AS [CourseMident],
       ocaslr_transcriptid AS [TranscriptId],
       ocaslr_grade AS [Grade],
       ocaslr_coursestatusid AS [CourseStatusId],
       ocaslr_coursetypeid AS [CourseTypeId],
       CreatedOn AS [CreatedOn],
       ModifiedOn AS [ModifiedOn],
       Ocaslr_modifiedbyuser AS [ModifiedBy],
       ocaslr_gradetypeid AS [GradeTypeId],
       ocaslr_coursedeliveryid AS [CourseDeliveryId],
       ocaslr_creditvalue AS [Credit],
       statecode AS [StateCode]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_ontariostudentcoursecreditBase]
