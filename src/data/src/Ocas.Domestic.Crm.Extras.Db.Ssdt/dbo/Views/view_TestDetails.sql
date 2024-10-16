﻿CREATE VIEW [dbo].[view_TestDetails]
	AS
SELECT [Detail].[ocaslr_testdetailId] AS [Id]
     , [Detail].[ocaslr_testid] AS [TestId]
     , [Template].[ocaslr_englishdescription] AS [EnglishDescription]
     , [Template].[ocaslr_frenchdescription] AS [FrenchDescription]
     , [Detail].[ocaslr_grade] AS [Grade]
     , [Detail].[ocaslr_percentile] AS [Percentile]
     , [Detail].[statecode] AS [StateCode]
     , [Detail].[statuscode] AS [StatusCode]
     , [Template].[ocaslr_sortorder] AS [SortOrder]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_testdetailBase] [Detail]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_testtemplateBase] [Template]
        ON [Detail].[ocaslr_testtemplateid] = [Template].[ocaslr_testtemplateId]
