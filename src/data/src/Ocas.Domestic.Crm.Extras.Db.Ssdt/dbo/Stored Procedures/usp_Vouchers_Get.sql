CREATE PROCEDURE [dbo].[usp_Vouchers_Get]
    @StateCode BIT = 0,
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @Code NVARCHAR(10) = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT [ocaslr_voucherId] AS [Id]
    , [ocaslr_Order] AS [OrderId]
    , [ocaslr_OrderItem] AS [OrderDetailIdString]
    , [ocaslr_Product] AS [ProductId]
    , [ocaslr_name] AS [Code]
    , [ocaslr_State] AS [VoucherState]
    , [ocaslr_Value] AS [Value]
    , [ocaslr_Applicant] AS [ApplicantId]
    , [ocaslr_Application] AS [ApplicationId]
    , [ocaslr_ShoppingCart] AS [ShoppingCartId]
    , [ocaslr_ShoppingCartItem] AS [ShoppingCartDetailIdString]
    , [ocaslr_Source] AS [SourceId]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_voucherBase]
WHERE (@StateCode IS NULL OR [statecode] = @StateCode) AND
    (@Id IS NULL OR [ocaslr_voucherId] = @Id) AND
    (@ApplicationId IS NULL OR [ocaslr_Application] = @ApplicationId) AND
    (@Code IS NULL OR [ocaslr_name] = @Code)
