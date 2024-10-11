CREATE PROCEDURE [dbo].[usp_Contact_GetSubjectId]
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ISNULL([Contact].[ocaslr_userid], [Contact].[OCASLR_UserName]) AS [SubjectId]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ContactBase] [Contact]
WHERE [Contact].[ContactId] = @Id AND
    [Contact].[StateCode] = 0 AND
    [Contact].[StatusCode] = 1 AND
    [Contact].[OCASLR_ContactType] = 1
