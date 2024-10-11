CREATE PROCEDURE [dbo].[usp_ShoppingCarts_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [ShoppingCart].[OpportunityId] AS [Id]
     , [ShoppingCart].[PriceLevelId] AS [PriceLevelId]
     , [ShoppingCart].[TransactionCurrencyId] AS [TransactionCurrencyId]
     , [Application].[ocaslr_applicantid] AS [ApplicantId]
     , [Application].[ocaslr_applicationId] AS [ApplicationId]
     , [ApplicationCycle].[ocaslr_applicationcyclestatusid] AS [ApplicationCycleStatusId]
     -- following fields are mapped to ShoppingCart.Details
     , [ShoppingCartDetail].[OpportunityProductId] AS [Id]
     , [ShoppingCartDetail].[OpportunityId] AS [ShoppingCartId]
     , CASE
         WHEN [ShoppingCartDetail].[ocaslr_transcriptrequestid] IS NOT NULL THEN [TranscriptRequest].[ocaslr_name]
         WHEN [ShoppingCartDetail].[ocaslr_Voucher] IS NOT NULL THEN [Voucher].[ocaslr_name]
         WHEN [Product].[ocaslr_servicetype] = 5 THEN
            CASE @Locale
                WHEN 0 THEN [Product].[Description]
                WHEN 1 THEN [Product].[ocaslr_descriptionfrench]
                ELSE NULL
            END
         WHEN [ShoppingCartDetail].[ocaslr_programchoiceid] IS NOT NULL THEN [ProgramChoice].[ocaslr_name]
         WHEN [Product].[Name] = 'APPLICATION FEE-OCAS' THEN 'Application Fee'
         ELSE NULL
       END AS [Description]
     , CASE
         WHEN [ShoppingCartDetail].[ocaslr_transcriptrequestid] IS NOT NULL THEN 'TranscriptRequestFee'
         WHEN [ShoppingCartDetail].[ocaslr_Voucher] IS NOT NULL THEN 'Voucher'
         WHEN [Product].[ocaslr_servicetype] = 5 THEN 'SupplementalApplicationFee'
         WHEN [ShoppingCartDetail].[ocaslr_programchoiceid] IS NOT NULL THEN 'ProgramChoice'
         WHEN [Product].[Name] = 'APPLICATION FEE-OCAS' THEN 'ApplicationFee'
         ELSE 'Unknown'
       END AS [Type]
     , CASE
         WHEN [ShoppingCartDetail].[ocaslr_transcriptrequestid] IS NOT NULL THEN [ShoppingCartDetail].[ocaslr_transcriptrequestid]
         WHEN [ShoppingCartDetail].[ocaslr_Voucher] IS NOT NULL THEN [ShoppingCartDetail].[ocaslr_Voucher]
         WHEN [Product].[ocaslr_servicetype] = 5 THEN [Product].[ocaslr_partnerid]
         WHEN [ShoppingCartDetail].[ocaslr_programchoiceid] IS NOT NULL THEN [ShoppingCartDetail].[ocaslr_programchoiceid]
         WHEN [Product].[Name] = 'APPLICATION FEE-OCAS' THEN [ShoppingCartDetail].[ocaslr_applicationid]
         ELSE NULL
       END AS [ReferenceId]
     , [ShoppingCartDetail].[BaseAmount] AS [Amount]
     , [ShoppingCartDetail].[Quantity] AS [Quantity]
     , [ShoppingCartDetail].[UoMId] AS [UomId]
     , [ShoppingCartDetail].[ProductId] AS [ProductId]
     , [ShoppingCartDetail].[ocaslr_applicationid] AS [ApplicationId]
     , [ShoppingCartDetail].[ocaslr_programchoiceid] AS [ProgramChoiceId]
     , [ShoppingCartDetail].[ocaslr_Voucher] AS [VoucherId]
     , [ShoppingCartDetail].[ocaslr_transcriptrequestid] AS [TranscriptRequestId]
     , [ShoppingCartDetail].[ManualDiscountAmount] AS [ManualDiscountAmount]
     , [ShoppingCartDetail].[PricePerUnit] AS [PricePerUnit]
     , [Product].[Name] AS [ProductName]
     , [Voucher].[ocaslr_Product] AS [VoucherProductId]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[OpportunityBase] [ShoppingCart]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationBase] [Application]
        ON [ShoppingCart].[ocaslr_applicationid] = [Application].[ocaslr_applicationId]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcycleBase] [ApplicationCycle]
        ON [Application].[ocaslr_applicationcycleid] = [ApplicationCycle].[ocaslr_applicationcycleId]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[OpportunityProductBase] [ShoppingCartDetail]
        ON [ShoppingCart].[OpportunityId] = [ShoppingCartDetail].[OpportunityId]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductBase] [Product]
        ON [ShoppingCartDetail].[ProductId] = [Product].[ProductId]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_transcriptrequestBase] [TranscriptRequest]
        ON [ShoppingCartDetail].[ocaslr_transcriptrequestid] = [TranscriptRequest].[ocaslr_transcriptrequestId]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_voucherBase] [Voucher]
        ON [ShoppingCartDetail].[ocaslr_Voucher] = [Voucher].[ocaslr_voucherId]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programchoiceBase] [ProgramChoice]
        ON [ShoppingCartDetail].[ocaslr_programchoiceid] = [ProgramChoice].[ocaslr_programchoiceId]
WHERE (@StateCode IS NULL OR [ShoppingCart].[StateCode] = @StateCode) AND
    (@Id IS NULL OR [ShoppingCart].[OpportunityId] = @Id) AND
    (@ApplicationId IS NULL OR [Application].[ocaslr_applicationId] = @ApplicationId)