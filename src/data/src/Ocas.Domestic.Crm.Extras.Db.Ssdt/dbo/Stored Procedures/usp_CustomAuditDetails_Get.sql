CREATE PROCEDURE [dbo].[usp_CustomAuditDetails_Get]
    @Id UNIQUEIDENTIFIER = NULL,
    @AuditId UNIQUEIDENTIFIER = NULL
AS

SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE @sqlCommand nvarchar(max)
       ,@paramListTerms nvarchar(max);

SET @sqlCommand = 'SELECT * FROM [dbo].[view_CustomAuditDetails] [CustomAuditDetails] WHERE 1=1 ';

IF (@Id IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [Id] = @Id';

IF (@AuditId IS NOT NULL)
    SET @sqlCommand = @sqlCommand + ' AND [CustomAuditId] = @AuditId';

SET @paramListTerms = '@Id UNIQUEIDENTIFIER, @AuditId UNIQUEIDENTIFIER';

EXECUTE sp_executesql @sqlCommand, @paramListTerms
    ,@Id = @Id
    ,@AuditId = @AuditId