CREATE PROCEDURE [dbo].[usp_SupportingDocumentBinaryData_Get]
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT TOP 1 DocumentBody
            ,ObjectId AS Id
            ,MimeType AS MimeType
            ,CreatedOn AS CreatedOn
            ,[FileName] AS [FileName]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[AnnotationBase]
WHERE (@Id IS NULL AND ObjectId IS NULL) OR
      ObjectId = @Id
ORDER BY CreatedOn DESC
