﻿CREATE VIEW [dbo].[view_Educations]
	AS SELECT [Education].[ocaslr_educationId] AS [Id]
    , [Education].[ocaslr_applicantid] AS [ApplicantId]
    , [Education].[ocaslr_academicupgrade] AS [AcademicUpgrade]
    , [Education].[ocaslr_countryid] AS [CountryId]
    , [Education].[ocaslr_provinceid] AS [ProvinceId]
    , [Education].[ocaslr_cityid] AS [CityId]
    , [Education].[ocaslr_institutetypeid] AS [InstituteTypeId]
    , [Education].[ocaslr_school] AS [InstituteId]
    , ISNULL([School].[Name], [Education].[ocaslr_institutename]) AS [InstituteName]
    , [Education].[ocaslr_currentlyattending] AS [CurrentlyAttending]
    , [Education].[ocaslr_graduatestatus] AS [Graduated]
    , [Education].[ocaslr_oen] AS [OntarioEducationNumber]
    , [Education].[ocaslr_studentnumber] AS [StudentNumber]
    , [Education].[ocaslr_firstnameonrecord] AS [FirstNameOnRecord]
    , [Education].[ocaslr_lastnameonrecord] AS [LastNameOnRecord]
    , [Education].[ocaslr_major] AS [Major]
    , [Education].[ocaslr_levelachievedid] AS [LevelAchievedId]
    , [Education].[ocaslr_lastgradecompletedid] AS [LastGradeCompletedId]
    , [Education].[ocaslr_credentialid] AS [CredentialId]
    , [Education].[ocaslr_credentialreceivedother] AS [OtherCredential]
    , [Education].[ocaslr_levelofstudiesid] AS [LevelOfStudiesId]
    , LEFT(CONVERT(VARCHAR(50), [ocaslr_attendedfromdate], 20), 7) AS [AttendedFrom]
    , LEFT(CONVERT(VARCHAR(50), [ocaslr_attendedtodate], 20), 7) AS [AttendedTo]
    , [Education].[ocaslr_languageofinstruction] AS [LanguageOfInstruction]
    , [Education].[CreatedOn] AS [CreatedOn]
    , [Education].[ModifiedOn] AS [ModifiedOn]
    , [Education].[ocaslr_modifiedbyuser] AS [ModifiedBy]
    , IIF(EXISTS (SELECT 1 FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_transcriptrequestBase] WHERE ocaslr_educationid = [Education].[ocaslr_educationId]), 1, 0) AS [HasTranscripts]
    , IIF(EXISTS (SELECT 1 FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationBase] A JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_applicationstatusBase] S ON A.ocaslr_applicationstatusid = S.Ocaslr_applicationstatusId WHERE A.ocaslr_applicantid =[Education].[ocaslr_applicantid] AND S.Ocaslr_code = '1'), 1, 0) AS [HasPaidApplication]
    , IIF(EXISTS (SELECT 1 FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_educationBase] WHERE [statecode] = 0 AND [statuscode] = 1 AND [ocaslr_applicantid] = [Education].[ocaslr_applicantid] AND [ocaslr_educationId] <> [Education].[ocaslr_educationId]), 1, 0) AS [HasMoreThanOneEducation]
    , [Education].[statecode] AS [StateCode]
    , [Education].[statuscode] AS [StatusCode]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_educationBase] [Education]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] [School]
        ON [Education].[ocaslr_school] = [School].[AccountId]
