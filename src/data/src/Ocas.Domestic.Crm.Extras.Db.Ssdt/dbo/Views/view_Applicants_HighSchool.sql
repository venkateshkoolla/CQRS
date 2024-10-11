CREATE VIEW [dbo].[view_Applicants_HighSchool]
	AS
SELECT DISTINCT [E].[ocaslr_applicantid] AS [ApplicantId]
       , [A].[ocaslr_mident] AS [Mident]
       , [B].[ocaslr_boardid] AS [BoardMident]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_educationBase] E 
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] A ON E.ocaslr_school  = A.AccountId
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] [B] ON A.[ParentAccountId] = [B].[AccountId]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ContactBase] C ON E.ocaslr_applicantid = C.ContactId
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_accountstatusBase] [AccStatus] ON [C].[ocaslr_accountstatusid] = [AccStatus].[ocaslr_accountstatusId]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_institutetypeBase] I ON E.ocaslr_institutetypeid = I.ocaslr_institutetypeId
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_provincestateBase] P ON E.ocaslr_provinceid = P.ocaslr_provincestateId
WHERE [I].[ocaslr_code] = 'HS' AND 
        [P].[ocaslr_code] = 'ON' AND
        [E].[ocaslr_currentlyattending] = 1 AND
        [C].[StatusCode] = 1 AND
        [C].[StateCode] = 0 AND
        [AccStatus].[ocaslr_code] = 1 AND
        ISNULL([C].[ocaslr_highschoolgraduated], 0) = 0
