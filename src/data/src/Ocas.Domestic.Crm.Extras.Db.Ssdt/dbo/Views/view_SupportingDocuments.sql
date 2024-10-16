﻿CREATE VIEW [dbo].[view_SupportingDocuments]
	AS
SELECT [ocaslr_supportingdocumentId] AS Id
      ,[Ocaslr_BatchName] AS BatchName
      ,[ocaslr_applicantid] AS ApplicantId
      ,[ocaslr_officialsignatureid] AS OfficialSignatureId
      ,[ocaslr_landedondate] AS LandedOnDate
      ,[ocaslr_credentialcodeid] AS CredentialCodeId
      ,[Ocaslr_boxnumber] AS BoxNumber 
      ,[ocaslr_completeid] AS CompleteId
      ,[ocaslr_level] AS [Level]
      ,[Ocaslr_Availability] AS [Availability]
      ,[ocaslr_name] AS [Name]
      ,[ocaslr_originalid] AS OriginalId
      ,[ocaslr_temporaryid] AS TemporaryId
      ,[Ocaslr_Surname] AS Surname
      ,[ocaslr_officialid] As OfficialId
      ,[ocaslr_graduationdate] As GraduationDate
      ,[ocaslr_cicclientid] AS CicClient
      ,[ocaslr_agencyid] AS AgencyId
      ,[ocaslr_programlevelid] AS ProgramLevelId
      ,[ocaslr_documenttypeid] AS DocumentTypeId
      ,[ocaslr_scanuserid] AS ScanUserId
      ,[ocaslr_expirydate] As ExpiryDate
      ,[ocaslr_documentnumber] AS DocumentNumber
      ,[Ocaslr_ScanDate] AS ScanDate
      ,[ocaslr_levelachievedid] AS LevelAchievedId
      ,[Ocaslr_Date_Received] AS DateReceived
      ,[ocaslr_issuedate] AS IssueDate
      ,[ocaslr_instituteid] AS InstituteId
      ,[ocaslr_provinceid] AS ProvinceId
      ,[ocaslr_class] AS Class
      ,[ocaslr_keydate] AS KeyDate
      ,[ocaslr_DocumentSourceId] AS DocumentSourceId
      ,[CreatedOn] AS [CreatedOn]
      ,[ModifiedOn] AS [ModifiedOn]
      ,[statecode] AS [StateCode]
      ,[statuscode] AS [StatusCode]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_supportingdocumentBase]
