CREATE PROCEDURE [dbo].[usp_Accounts_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @AccountType INT = NULL,
    @CollegeStatusCode CHAR(1) = NULL,
    @ParentId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [AccountId] AS [Id]
       , CASE
           WHEN ocaslr_accounttype = 5
                AND ocaslr_brandname IS NOT NULL
                AND ocaslr_brandname <> '' THEN ocaslr_brandname
           ELSE Acc.[Name]
         END AS [Name]
       , CASE @AccountType
           WHEN 3 THEN [ocaslr_collegecode]
           WHEN 5 THEN [ocaslr_campuscode]
           WHEN 6 THEN [Acc].[ocaslr_dwcode]
           ELSE NULL
         END AS [Code]
       , [ParentAccountId] AS [ParentId]
       , Ss.[ocaslr_schoolstatusId] AS [SchoolStatusId]
       , ocaslr_etms AS [HasEtms]
       , [AccPPL].Amount AS [TranscriptFee]
       , [Config].AllowCba
       , [Config].AllowCbaBrandConfig
       , [Config].AllowCbaMultiCollegeApply
       , [Config].AllowCbaReferralCodeAsSource
       , [Config].AllowCbaSearch
       , [Config].AllowCbaCobranding
       , ISNULL(Acc.[Ocaslr_ShowinEducationDeclaration], CAST(1 AS BIT)) AS [ShowInEducation]
        -- following fields are mapped to Account.MailingAddress
       , 1 AS [MailingAddressSplit]
       , [Address].[Line1] AS [Street]    
       , [Address].[City]          
       , [Address].[StateOrProvince] [ProvinceState]
       , [Address].[PostalCode]
       , [Address].[Country]
FROM   [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] AS Acc
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_schoolstatusBase] AS Ss
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
            ProductBase.ocaslr_servicetype NOT IN (3, 4, 5) AND
            ProductBase.ocaslr_partnerid IS NOT NULL) [AccPPL] ON [AccountId] = [AccPPL].ocaslr_partnerid AND [AccPPL].[RowNum] = 1
    LEFT JOIN (
         SELECT ocaslr_partnerid
              , [100000000] AS [AllowCba]
              , [100000001] AS [AllowCbaSearch]
              , [100000002] AS [AllowCbaMultiCollegeApply]
              , [100000003] AS [AllowCbaBrandConfig]
              , [100000004] AS [AllowCbaReferralCodeAsSource]
              , [100000005] AS [AllowCbaCobranding]
         FROM (
                SELECT ocaslr_partnerid, ocaslr_SettingKey, cast(ocaslr_SettingValue as int) AS ocaslr_SettingValue
                FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_partnerexperienceconfigurationBase]) p
                PIVOT(MAX(ocaslr_SettingValue)
                FOR ocaslr_SettingKey in ([100000000],[100000001],[100000002],[100000003],[100000004],[100000005])) AS PivotTable) Config
                ON Acc.[AccountId] = Config.[ocaslr_partnerid]
WHERE  (@StateCode IS NULL OR Acc.StateCode = @StateCode) AND
       (@Id IS NULL OR [AccountId] = @Id) AND
       (@AccountType IS NULL OR ocaslr_accounttype = @AccountType) AND
       (@CollegeStatusCode IS NULL OR Ss.ocaslr_code = @CollegeStatusCode) AND
       (@ParentId IS NULL OR ParentAccountId = @ParentId) AND
       (Ss.ocaslr_code IS NULL OR Ss.ocaslr_code NOT IN ( 'D', 'U' ))
ORDER BY Acc.[ParentAccountId], Acc.[Name]