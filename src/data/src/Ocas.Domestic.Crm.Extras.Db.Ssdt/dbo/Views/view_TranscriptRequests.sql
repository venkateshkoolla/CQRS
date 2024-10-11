CREATE VIEW [dbo].[view_TranscriptRequests]
    AS
SELECT [ocaslr_transcriptrequestId] AS [Id]
      ,[ocaslr_peterequestlogid] AS [PeteRequestLogId]
      ,[ocaslr_fromschooltype] AS [FromSchoolType]
      ,TR.[ocaslr_name] AS [Name]
      ,[ocaslr_applicationid] AS [ApplicationId]
      ,[ocaslr_educationid] AS [EducationId]
      ,[ocaslr_toschoolid] AS [ToSchoolId]
      ,SchoolTo.[Name] AS [ToSchoolName]
      ,[ocaslr_applicantid] AS [ApplicantId]
      ,[ocaslr_fromschoolid] AS [FromSchoolId]
      ,SchoolFrom.[Name] AS [FromSchoolName]
      ,[ocaslr_transcripttransmissionid] AS [TranscriptTransmissionId]
      ,[ocaslr_transcriptrequeststatusid] AS [TranscriptRequestStatusId]
      ,[Ocaslr_eTMSTranscriptRequestId] AS [eTMSTranscriptRequestId]
      ,COALESCE([AccPPL].Amount, [PaccPPL].Amount) AS [TranscriptFee]
      ,TR.[CreatedOn]
      ,TR.[ModifiedOn]
      ,[ocaslr_modifiedbyuser] AS [ModifiedBy]
      ,TR.[statuscode]
      ,TR.[statecode]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_transcriptrequestBase] AS TR
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] AS SchoolFrom ON TR.[ocaslr_fromschoolid] = SchoolFrom.[AccountId]
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] AS SchoolTo ON TR.[ocaslr_toschoolid] = SchoolTo.[AccountId]
    LEFT JOIN (
        SELECT ocaslr_partnerid, Amount, ROW_NUMBER() OVER (PARTITION BY ProductBase.ocaslr_partnerid ORDER BY ocaslr_effectivedate DESC) [RowNum]
        FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductBase]
            LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductPriceLevelBase] ON 
                ProductPriceLevelBase.ProductId = ProductBase.ProductId AND 
                ProductPriceLevelBase.PriceLevelId = ProductBase.PriceLevelId
        WHERE ProductBase.StateCode = 0 AND
            ProductBase.ocaslr_effectivedate <= GETUTCDATE() AND
            ProductBase.ocaslr_servicetype NOT IN (3, 4, 5) AND
            ProductBase.ocaslr_partnerid IS NOT NULL) [AccPPL] ON [SchoolFrom].AccountId = [AccPPL].ocaslr_partnerid AND [AccPPL].[RowNum] = 1
    LEFT JOIN (
        SELECT ocaslr_partnerid, Amount, ROW_NUMBER() OVER (PARTITION BY ProductBase.ocaslr_partnerid ORDER BY ocaslr_effectivedate DESC) [RowNum]
        FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductBase]
            LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductPriceLevelBase] ON 
                ProductPriceLevelBase.ProductId = ProductBase.ProductId AND 
                ProductPriceLevelBase.PriceLevelId = ProductBase.PriceLevelId
        WHERE ProductBase.StateCode = 0 AND
            ProductBase.ocaslr_effectivedate <= GETUTCDATE() AND
            ProductBase.ocaslr_servicetype NOT IN (3, 4, 5) AND
            ProductBase.ocaslr_partnerid IS NOT NULL) [PaccPPL] ON [SchoolFrom].ParentAccountId = [PaccPPL].ocaslr_partnerid AND [PaccPPL].[RowNum] = 1
