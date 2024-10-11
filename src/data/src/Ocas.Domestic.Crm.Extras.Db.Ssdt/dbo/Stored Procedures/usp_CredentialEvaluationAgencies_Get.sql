CREATE PROCEDURE [dbo].[usp_CredentialEvaluationAgencies_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

-- From Applicant API
-- https://dev.azure.com/ocas/OCAS%20Portfolio/Codebusters/_git/domesticapi?path=%2Fsrc%2FOCAS.Connector.CRM.WCF%2FServices%2FLookup%2FLookupWebService.svc.cs&version=GBmaster&line=526&lineStyle=plain&lineEnd=553&lineStartColumn=17&lineEndColumn=61

SELECT [AccountBase].[AccountId] AS Id,
    [AccountBase].[ParentAccountId] AS [ParentId],
    [AccountBase].[Name],
    [AccountBase].[ocaslr_collegecode] AS [Code],
    [AccountBase].[ocaslr_schoolstatusid] [SchoolStatusId],
    [AccountBase].[ocaslr_etms] [HasEtms],
    [Address].[AddressTypeCode],
-- following fields are mapped to Account.MailingAddress
    1 AS [MailingAddressSplit],
    [Address].[Line1] AS [Street],
    [Address].City AS [AddressCity],
    [Address].StateOrProvince AS [AddressStateProvince],
    [Address].PostalCode AS [AddressPostalCode],
    [Address].Country AS [AddressCountry]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase]
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[CustomerAddressBase] [Address] ON ([AccountBase].[AccountId] = [Address].ParentId AND [Address].AddressNumber = 1)
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] [account_master_account] ON ([AccountBase].[MasterId] = [account_master_account].[AccountId])
    LEFT JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[AccountBase] [account_parent_account] ON ([AccountBase].[ParentAccountId] = [account_parent_account].[AccountId])
WHERE [AccountBase].[ocaslr_accounttype] = 4 AND
    (@StateCode IS NULL OR [AccountBase].[StateCode] = @StateCode) AND
    (@Id IS NULL OR [AccountBase].[AccountId] = @Id)
