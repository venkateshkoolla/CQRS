CREATE PROCEDURE [dbo].[usp_Contact_IsActive]
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT TOP 1 1
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ContactBase] [Contact]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_accountstatusBase] [AccStatus] 
        ON [Contact].[ocaslr_accountstatusid] = [AccStatus].[ocaslr_accountstatusId]
WHERE   [Contact].[ContactId] = @Id AND
        [Contact].[StateCode] = 0 AND
        [Contact].[StatusCode] = 1 AND
        [Contact].[OCASLR_ContactType] = 1 AND
        [AccStatus].[ocaslr_code] = 1
