CREATE PROCEDURE [dbo].[usp_Currencies_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

-- https://dev.azure.com/ocas/OCAS%20Portfolio/Codebusters/_git/domesticapi?path=%2Fsrc%2FOCAS.Connector.CRM.WCF%2FServices%2FLookup%2FLookupWebService.svc.cs&version=GBmaster&line=1190&lineStyle=plain&lineEnd=1193&lineStartColumn=1&lineEndColumn=53

SELECT [TransactionCurrencyId] AS Id,
    [CurrencyName] As [Name],
    [ISOCurrencyCode] AS [Code]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[TransactionCurrencyBase]
WHERE (@StateCode IS NULL OR StateCode = @StateCode) AND
    (@Id IS NULL OR [TransactionCurrencyId] = @Id)
