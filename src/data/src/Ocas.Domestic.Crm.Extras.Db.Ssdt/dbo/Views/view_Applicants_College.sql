CREATE VIEW [dbo].[view_Applicants_College]
	AS
SELECT DISTINCT [choice].[ocaslr_applicantid] AS [ApplicantId]
       , [College].[ocaslr_collegecode] AS [CollegeCode]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programchoiceBase] Choice
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programintakeBase] Intake ON Choice.ocaslr_programintakeid = Intake.ocaslr_programintakeId
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programBase] Program ON Intake.ocaslr_programid = Program.ocaslr_programId
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_collegeapplicationcycleBase] CollegeAppCycle ON CollegeAppCycle.ocaslr_collegeapplicationcycleId = Program.ocaslr_collegeapplicationcycleid
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationBase] [Application] ON [Application].ocaslr_applicationId = Choice.ocaslr_applicationid
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcycleBase] [AppCycle] ON [Application].ocaslr_applicationcycleid = [AppCycle].ocaslr_applicationcycleId
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcyclestatusBase] [AppCycleStatus] ON [AppCycle].ocaslr_applicationcyclestatusid = [AppCycleStatus].ocaslr_applicationcyclestatusId
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] College ON CollegeAppCycle.ocaslr_collegeid = College.AccountId
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ContactBase] Contact ON [choice].[ocaslr_applicantid] = Contact.ContactId
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_accountstatusBase] [AccStatus] ON Contact.[ocaslr_accountstatusid] = [AccStatus].[ocaslr_accountstatusId]
WHERE [choice].[statecode] = 0 AND
      Contact.[StatusCode] = 1 AND
      Contact.[StateCode] = 0 AND
      [AccStatus].[ocaslr_code] = 1 AND
      [AppCycleStatus].[ocaslr_code] = N'A'
