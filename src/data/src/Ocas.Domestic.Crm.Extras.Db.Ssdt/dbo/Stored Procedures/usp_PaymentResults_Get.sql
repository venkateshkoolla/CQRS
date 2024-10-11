CREATE PROCEDURE [dbo].[usp_PaymentResults_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_paymentresultId AS Id
    , ocaslr_code AS Code
    , ocaslr_name AS [Name]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_paymentresultBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@Id IS NULL OR ocaslr_paymentresultId = @Id)
ORDER BY ocaslr_name
