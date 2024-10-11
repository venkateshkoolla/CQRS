CREATE PROCEDURE [dbo].[usp_Contact_IsDuplicateEmail]
    @Id UNIQUEIDENTIFIER = NULL,
    @EmailAddress VARCHAR(100) = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT TOP 1 1
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ContactBase] [Contact]
WHERE [Contact].[ContactId] <> @Id AND
    ([Contact].[OCASLR_UserName] = @EmailAddress OR [Contact].[EMailAddress1] = @EmailAddress) AND
    [Contact].[StateCode] = 0 AND
    [Contact].[StatusCode] = 1 AND
    [Contact].[OCASLR_ContactType] = 1
