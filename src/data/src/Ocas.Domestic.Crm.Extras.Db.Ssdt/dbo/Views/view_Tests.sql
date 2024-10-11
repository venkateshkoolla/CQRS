﻿CREATE VIEW [dbo].[view_Tests]
	AS
SELECT [Test].[ocaslr_testId] AS [Id]
     , [Test].[ocaslr_applicantid] AS [ApplicantId]
     , [Test].[ocaslr_testtypeid] AS [TestTypeId]
     , [Test].[ocaslr_countryid] AS [CountryId]
     , [Test].[ocaslr_provincestate] AS [ProvinceStateName]
     , [Test].[ocaslr_provincestateid] AS [ProvinceStateId]
     , [Test].[ocaslr_city] AS [CityName]
     , [Test].[ocaslr_cityid] AS [CityId]
     , [Test].[ocaslr_datetesttaken] AS [DateTestTaken]
     , [Test].[ocaslr_description] AS [Description]
     , [Test].[ocaslr_official] AS [IsOfficial]
     , [Test].[CreatedOn] AS [CreatedOn]
     , [Test].[ModifiedOn] AS [ModifiedOn]
     , [Test].[ocaslr_modifiedbyuser] AS [ModifiedBy]
     , [NormingGroup].[Ocaslr_englishdescription] AS [EnglishNormingGroupName]
     , [NormingGroup].[Ocaslr_frenchdescription] AS [FrenchNormingGroupName]
     , [ApplicationCycle].[ocaslr_name] AS [ApplicationCycleName]
     , [Test].[statecode] AS [StateCode]
     , [Test].[statuscode] AS [StatusCode]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_testBase] [Test]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[Ocaslr_norminggroupBase] [NormingGroup]
        ON [Test].[ocaslr_norminggroupid] = [NormingGroup].[Ocaslr_norminggroupId]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcycleBase] [ApplicationCycle]
        ON [Test].[Ocaslr_ApplicationCycleId] = [ApplicationCycle].[ocaslr_applicationcycleId]
