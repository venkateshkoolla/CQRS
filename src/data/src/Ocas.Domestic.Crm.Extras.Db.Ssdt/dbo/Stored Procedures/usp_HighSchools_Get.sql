CREATE PROCEDURE [dbo].[usp_HighSchools_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @Locale INT = 0
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

-- https://dev.azure.com/ocas/OCAS%20Portfolio/Codebusters/_git/domesticapi?path=%2Fsrc%2FOCAS.Connector.CRM.WCF%2FServices%2FLookup%2FLookupWebService.svc.cs&version=GBmaster&line=1697&lineStyle=plain&lineEnd=1750&lineStartColumn=17&lineEndColumn=89

SELECT AccountBase.AccountId [Id],
    AccountBase.ParentAccountId [ParentId],
    AccountBase.ocaslr_schooltypeid [SchoolTypeId],
    AccountBase.ocaslr_mident [Code],
    CASE @Locale
        WHEN 0 THEN [ocaslr_schooltype_account].[ocaslr_englishdescription]
        WHEN 1 THEN [ocaslr_schooltype_account].[ocaslr_frenchdescription]
        ELSE NULL
    END AS [SchoolType],
    [account_parent_account].[Name] [SchoolBoardName],
    AccountBase.ocaslr_schoolid [SchoolId],
    AccountBase.[Name] [Name],
    AccountBase.ocaslr_schoolstatusid [SchoolStatusId],
    CASE @Locale
        WHEN 0 THEN [ocaslr_schoolstatus_account].ocaslr_englishdescription
        WHEN 1 THEN [ocaslr_schoolstatus_account].ocaslr_frenchdescription
        ELSE NULL
    END AS [SchoolStatus],
    AccountBase.ocaslr_etms AS [HasEtms],
    COALESCE([HsPPL].Amount, [SbPPL].Amount) AS [TranscriptFee],
    ISNULL(AccountBase.[Ocaslr_ShowinEducationDeclaration], CAST(1 AS BIT)) AS [ShowInEducation],
    [Address].AddressTypeCode,
    [AccountBase].[ocaslr_mident] AS [Mident],
    [account_parent_account].[ocaslr_boardid] AS [BoardMident],
    -- following fields are mapped to Account.MailingAddress
    1 AS [MailingAddressSplit],
    [Address].[Line1] AS [Street],
    [Address].City AS [City],
    [Address].StateOrProvince AS [ProvinceState],
    [Address].PostalCode AS [PostalCode],
    [Address].Country AS [Country]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase]
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[CustomerAddressBase] [Address]  ON (AccountBase.AccountId = [Address].ParentId AND [Address].AddressNumber = 1)
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] [account_master_account] ON (AccountBase.MasterId = [account_master_account].AccountId)
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] [account_parent_account] ON (AccountBase.ParentAccountId = [account_parent_account].AccountId)
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_schoolstatusBase] [ocaslr_schoolstatus_account] ON (AccountBase.ocaslr_schoolstatusid = [ocaslr_schoolstatus_account].ocaslr_schoolstatusId)
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_schooltypeBase] [ocaslr_schooltype_account] ON (AccountBase.ocaslr_schooltypeid = [ocaslr_schooltype_account].ocaslr_schooltypeId)
    LEFT JOIN (
        SELECT ocaslr_partnerid, Amount, ROW_NUMBER() OVER (PARTITION BY ProductBase.ocaslr_partnerid ORDER BY ocaslr_effectivedate DESC) [RowNum]
        FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductBase]
            LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductPriceLevelBase] ON 
                ProductPriceLevelBase.ProductId = ProductBase.ProductId AND 
                ProductPriceLevelBase.PriceLevelId = ProductBase.PriceLevelId
        WHERE ProductBase.StateCode = 0 AND
            ProductBase.ocaslr_effectivedate <= GETUTCDATE() AND
            ProductBase.ocaslr_servicetype NOT IN (3, 4, 5) AND
            ProductBase.ocaslr_partnerid IS NOT NULL) [HsPPL] ON AccountBase.AccountId = [HsPPL].ocaslr_partnerid AND [HsPPL].[RowNum] = 1
    LEFT JOIN (
        SELECT ocaslr_partnerid, Amount, ROW_NUMBER() OVER (PARTITION BY ProductBase.ocaslr_partnerid ORDER BY ocaslr_effectivedate DESC) [RowNum]
        FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductBase]
            LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductPriceLevelBase] ON 
                ProductPriceLevelBase.ProductId = ProductBase.ProductId AND 
                ProductPriceLevelBase.PriceLevelId = ProductBase.PriceLevelId
        WHERE ProductBase.StateCode = 0 AND
            ProductBase.ocaslr_effectivedate <= GETUTCDATE() AND
            ProductBase.ocaslr_servicetype NOT IN (3, 4, 5) AND
            ProductBase.ocaslr_partnerid IS NOT NULL) [SbPPL] ON AccountBase.ParentAccountId = [SbPPL].ocaslr_partnerid AND [SbPPL].[RowNum] = 1
WHERE (@StateCode IS NULL OR AccountBase.StateCode = @StateCode)
    AND AccountBase.ocaslr_accounttype = 2
    AND [ocaslr_schoolstatus_account].ocaslr_name IN ('Closed','Open')
    AND (@Id IS NULL OR AccountBase.AccountId = @Id)