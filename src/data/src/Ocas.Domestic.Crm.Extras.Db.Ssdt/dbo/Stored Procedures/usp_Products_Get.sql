CREATE PROCEDURE [dbo].[usp_Products_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @StateCode BIT = 0,
    @ProductServiceType TINYINT = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT TOP 1
    ProductBase.ProductId AS [Id],
    ProductBase.ProductNumber AS [Code],
    ProductBase.[Name],
    ProductBase.ocaslr_servicetype [ServiceType],
    ProductPriceLevelBase.Amount
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductBase]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ProductPriceLevelBase] ON ProductPriceLevelBase.ProductId = ProductBase.ProductId
WHERE (@StateCode IS NULL OR StateCode = @StateCode) AND
    (@ProductServiceType IS NULL OR ocaslr_servicetype = @ProductServiceType) AND
    (@Id IS NULL OR ProductBase.ProductId = @Id) AND
    ocaslr_effectivedate <= GETUTCDATE()
ORDER BY ocaslr_effectivedate DESC