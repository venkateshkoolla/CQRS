CREATE PROCEDURE [dbo].[usp_OrderDetails_Get]
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

    SET @sqlCommand =  'SELECT * FROM [dbo].[view_OrderDetails] WHERE 1= 1 ' ;

IF(@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND Id = @Id'

    SET @paramListTerms = '@Id UNIQUEIDENTIFIER';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    , @Id = @Id
