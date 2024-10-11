CREATE PROCEDURE [dbo].[usp_Contact_IsDuplicateDetails]
    @Id UNIQUEIDENTIFIER = NULL,
    @FirstName VARCHAR(50) = NULL,
    @LastName VARCHAR(50) = NULL,
    @BirthDate DATETIME = NULL
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

SELECT TOP 1 1
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ContactBase] [Contact]
WHERE [Contact].[ContactId] <> @Id AND
    (@FirstName IS NULL OR [Contact].[FirstName] = @FirstName) AND
    (@LastName IS NULL OR [Contact].[LastName] = @LastName) AND
    (@BirthDate IS NULL OR
        (DATEPART(YY, [Contact].[BirthDate]) = DATEPART(YY, @BirthDate) AND 
        DATEPART(MM, [Contact].[BirthDate]) = DATEPART(MM, @BirthDate) AND 
        DATEPART(DD, [Contact].[BirthDate]) = DATEPART(DD, @BirthDate))) AND
    [Contact].[StateCode] = 0 AND
    [Contact].[StatusCode] = 1 AND
    [Contact].[OCASLR_ContactType] = 1
