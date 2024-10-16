﻿CREATE VIEW [dbo].[view_AcademicRecords]
	AS
SELECT [AcademicData].[ocaslr_academicdataId] AS [Id]
      ,[AcademicData].[ocaslr_contactid] AS [ApplicantId]
      ,[AcademicData].[ocaslr_name] AS [Name]
      ,[AcademicData].[ocaslr_gradedate] AS [DateCredentialAchieved]
      ,[AcademicData].[CreatedOn] AS [CreatedOn]
      ,[AcademicData].[ModifiedOn] AS [ModifiedOn]
      ,[AcademicData].[ocaslr_modifiedbyuser] AS [ModifiedBy]
      ,[AcademicData].[ocaslr_sssn] AS [SSSN]
      ,[AcademicData].[ocaslr_studentid] AS [StudentId]
      ,[AcademicData].[ocaslr_yearsinossl] AS [YearsInOssl]
      ,[AcademicData].[ocaslr_mident] AS [Mident]
      ,[AcademicData].[ocaslr_gradstatuscode] AS [GradStatusCode]
      ,[AcademicData].[ocaslr_shmcompletionid] AS [ShsmCompletionId]
      ,[AcademicData].[ocaslr_communityinvolvementid] AS [CommunityInvolvementId]
      ,[AcademicData].[ocaslr_highesteducationid] AS [HighestEducationId]
      ,[AcademicData].[ocaslr_highskillsmajorid] AS [HighSkillsMajorId]
      ,[AcademicData].[ocaslr_literacytestid] AS [LiteracyTestId]
      ,[AcademicData].[statecode] AS [StateCode]
      ,[AcademicData].[statuscode] AS [StatusCode]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_academicdataBase] AS [AcademicData]
