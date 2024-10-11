CREATE PROCEDURE [dbo].[usp_ProgramChoices_Any]
    @ApplicationId UNIQUEIDENTIFIER = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT TOP 1 1
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programchoiceBase]
WHERE [ocaslr_applicationid] = @ApplicationId AND [statecode] = 0
