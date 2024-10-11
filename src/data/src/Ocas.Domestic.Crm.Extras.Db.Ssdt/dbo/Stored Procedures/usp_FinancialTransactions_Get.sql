CREATE PROCEDURE [dbo].[usp_FinancialTransactions_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @ApplicantId UNIQUEIDENTIFIER = NULL,
    @ApplicationId UNIQUEIDENTIFIER = NULL,
    @OrderId UNIQUEIDENTIFIER = NULL,
    @ReceiptId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

    SET @sqlCommand = 'SELECT [FTView].*, [ReceiptView].*
                       FROM [dbo].[view_FinancialTransactions] [FTView]
                       LEFT OUTER JOIN [dbo].[view_Receipts] [ReceiptView] ON [FTView].ocaslr_paymenttransactiondetailid = [ReceiptView].Id
                       WHERE 1=1 ';

IF(@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [FTView].Id = @Id';

IF (@ApplicationId IS NOT NULL)
    SET @sqlCommand  = @sqlCommand + ' AND [FTView].[ApplicationId] = @ApplicationId';

IF (@ApplicantId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [FTView].[ApplicantId] = @ApplicantId';

IF(@OrderId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [FTView].[Orderid] = @OrderId';

IF(@ReceiptId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [FTView].[ocaslr_paymenttransactiondetailid] = @ReceiptId';

    SET @sqlCommand = @sqlCommand + ' ORDER BY [FTView].[TransactionDate] DESC'
    SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @ApplicantId UNIQUEIDENTIFIER, @ApplicationId UNIQUEIDENTIFIER, @OrderId UNIQUEIDENTIFIER, @ReceiptId UNIQUEIDENTIFIER';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@ApplicantId = @ApplicantId
    ,@ApplicationId = @ApplicationId
    ,@OrderId = @OrderId
    ,@ReceiptId = @ReceiptId
