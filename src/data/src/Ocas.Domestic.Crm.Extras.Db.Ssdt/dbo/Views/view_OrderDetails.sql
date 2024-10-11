CREATE VIEW [dbo].[view_OrderDetails]
    AS
SELECT [OrderDetail].[SalesOrderDetailId] AS [Id]
     , [OrderDetail].[SalesOrderId] AS [OrderId]
     , CASE
         WHEN [OrderDetail].[ocaslr_transcriptrequestid] IS NOT NULL THEN [TranscriptRequest].[ocaslr_name]
         WHEN [OrderDetail].[ocaslr_Voucher] IS NOT NULL THEN [Voucher].[ocaslr_name]
         WHEN [Product].[ocaslr_servicetype] = 5 THEN 'Supplemental Application Fee'
         WHEN [OrderDetail].[ocaslr_programchoiceid] IS NOT NULL THEN [ProgramChoice].[ocaslr_name]
         WHEN [Product].[Name] = 'PROGRAM CHOICE-OCAS' THEN 'Program Choice' -- in case program choice is deleted
         WHEN [Product].[Name] = 'APPLICATION FEE-OCAS' THEN 'Application Fee'
         ELSE NULL
       END AS [Description]
     , CASE
         WHEN [OrderDetail].[ocaslr_transcriptrequestid] IS NOT NULL THEN 'TranscriptRequestFee'
         WHEN [OrderDetail].[ocaslr_Voucher] IS NOT NULL THEN 'Voucher'
         WHEN [Product].[ocaslr_servicetype] = 5 THEN 'SupplementalApplicationFee'
         WHEN [OrderDetail].[ocaslr_programchoiceid] IS NOT NULL THEN 'ProgramChoice'
         WHEN [Product].[Name] = 'PROGRAM CHOICE-OCAS' THEN 'ProgramChoice' -- in case program choice is deleted
         WHEN [Product].[Name] = 'APPLICATION FEE-OCAS' THEN 'ApplicationFee'
         ELSE 'Unknown'
       END AS [Type]
     , CASE
         WHEN [OrderDetail].[ocaslr_transcriptrequestid] IS NOT NULL THEN [OrderDetail].[ocaslr_transcriptrequestid]
         WHEN [OrderDetail].[ocaslr_Voucher] IS NOT NULL THEN [OrderDetail].[ocaslr_Voucher]
         WHEN [Product].[ocaslr_servicetype] = 5 THEN [Product].[ocaslr_partnerid]
         WHEN [OrderDetail].[ocaslr_programchoiceid] IS NOT NULL THEN [OrderDetail].[ocaslr_programchoiceid]
         WHEN [Product].[Name] = 'PROGRAM CHOICE-OCAS' THEN NULL -- in case program choice is deleted
         WHEN [Product].[Name] = 'APPLICATION FEE-OCAS' THEN [OrderDetail].[ocaslr_applicationid]
         ELSE NULL
       END AS [ReferenceId]
     , [OrderDetail].[BaseAmount] AS [Amount]
     , [OrderDetail].[Quantity] AS [Quantity]
     , [OrderDetail].[ProductId] AS [ProductId]
     , [OrderDetail].[ProductDescription] AS [ProductDescription]
     , [OrderDetail].[PricePerUnit] AS [PricePerUnit]
     , [OrderDetail].[ocaslr_Voucher] AS [VoucherId]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[SalesOrderDetailBase] [OrderDetail]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductBase] [Product]
        ON [OrderDetail].[ProductId] = [Product].[ProductId] AND [Product].[StateCode] = 0
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_transcriptrequestBase] [TranscriptRequest]
        ON [OrderDetail].[ocaslr_transcriptrequestid] = [TranscriptRequest].[ocaslr_transcriptrequestId] AND [TranscriptRequest].[statecode] = 0
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_voucherBase] [Voucher]
        ON [OrderDetail].[ocaslr_Voucher] = [Voucher].[ocaslr_voucherId] AND [Voucher].[statecode] = 0
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programchoiceBase] [ProgramChoice]
        ON [OrderDetail].[ocaslr_programchoiceid] = [ProgramChoice].[ocaslr_programchoiceId] AND [ProgramChoice].[statecode] = 0