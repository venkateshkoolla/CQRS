CREATE PROCEDURE [dbo].[usp_Universities_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [AccountId] AS [Id]
       , Acc.[Name]
       , [ocaslr_collegecode] AS [Code]
       , Ss.[ocaslr_schoolstatusId] AS [SchoolStatusId]
       , ocaslr_etms AS [HasEtms]
       , [AccPPL].Amount AS [TranscriptFee]
       , ISNULL(Acc.[Ocaslr_ShowinEducationDeclaration], CAST(1 AS BIT)) AS [ShowInEducation]
        -- following fields are mapped to Account.MailingAddress
       , 1 AS [MailingAddressSplit]
       , [Address].[Line1] AS [Street]    
       , [Address].[City]          
       , [Address].[StateOrProvince] [ProvinceState]
       , [Address].[PostalCode]
       , [Address].[Country]
FROM   [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] AS Acc
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_schoolstatusBase] AS Ss
        ON Acc.ocaslr_schoolstatusid = Ss.ocaslr_schoolstatusId
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[CustomerAddressBase] As [Address]
        ON ([AccountId] = [Address].ParentId AND [Address].AddressNumber = 1)
    LEFT JOIN (
        SELECT ocaslr_partnerid, Amount, ROW_NUMBER() OVER (PARTITION BY ProductBase.ocaslr_partnerid ORDER BY ocaslr_effectivedate DESC) [RowNum]
        FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductBase]
            LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductPriceLevelBase] ON 
                ProductPriceLevelBase.ProductId = ProductBase.ProductId AND 
                ProductPriceLevelBase.PriceLevelId = ProductBase.PriceLevelId
        WHERE ProductBase.StateCode = 0 AND
            ProductBase.ocaslr_effectivedate <= GETUTCDATE() AND
            ProductBase.ocaslr_servicetype NOT IN (3, 4) AND
            ProductBase.ocaslr_partnerid IS NOT NULL) [AccPPL] ON [AccountId] = [AccPPL].ocaslr_partnerid AND [AccPPL].[RowNum] = 1
WHERE  (@StateCode IS NULL OR Acc.StateCode = @StateCode) AND
       (@Id IS NULL OR [AccountId] = @Id) AND
       (ocaslr_accounttype = 8) AND
       (Ss.ocaslr_code IN ( 'O', 'C' ))
ORDER  BY Acc.[Name]
