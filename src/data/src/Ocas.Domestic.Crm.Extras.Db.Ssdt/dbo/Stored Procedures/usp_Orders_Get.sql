CREATE PROCEDURE [dbo].[usp_Orders_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @CustomerId UNIQUEIDENTIFIER = NULL,
    @OrderNumber NVARCHAR(100) = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

    SET @sqlCommand = 'SELECT [Orders].*, [OrderDetails].*
                       FROM [dbo].[view_Orders] [Orders]
                           LEFT OUTER JOIN [dbo].[view_OrderDetails] [OrderDetails]
                               ON [Orders].[Id] = [OrderDetails].[OrderId]
                       WHERE 1=1 ';

IF(@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Orders].Id = @Id';

IF (@CustomerId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Orders].[CustomerId] = @CustomerId';

IF (@OrderNumber IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Orders].[OrderNumber] = @OrderNumber';

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @CustomerId UNIQUEIDENTIFIER, @OrderNumber NVARCHAR(100)';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@CustomerId = @CustomerId
    ,@OrderNumber = @OrderNumber
