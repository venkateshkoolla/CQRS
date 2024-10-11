CREATE PROCEDURE [dbo].[usp_AcceptedPrivacyStatements_Get]
    @StateCode BIT = 0,
    @ContactId UNIQUEIDENTIFIER = NULL,
    @PrivacyStatementId UNIQUEIDENTIFIER = NULL,
    @Id UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT ocaslr_acceptedprivacystatementId AS Id
    , ocaslr_name AS [Name]
    , ocaslr_accepteddate AS [AcceptedDate]
    , ocaslr_privacystatementid AS [PrivacyStatementId]
    , ocaslr_contactid AS [ContactId] 
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_acceptedprivacystatementBase]
WHERE (@StateCode IS NULL OR statecode = @StateCode) AND
    (@ContactId IS NULL OR ocaslr_contactid = @ContactId) AND
    (@PrivacyStatementId IS NULL OR ocaslr_privacystatementid = @PrivacyStatementId) AND
    (@Id IS NULL OR ocaslr_acceptedprivacystatementId = @Id)