CREATE PROCEDURE [dbo].[usp_Contact_GetCompletedSteps]
    @Id UNIQUEIDENTIFIER
AS
SET NOCOUNT ON;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;

DECLARE
    @isPersonalInformationCompleted BIT = 0,
    @isContactInformationCompleted BIT = 0,
    @isCitizenshipAndResidencyCompleted BIT = 0,
    @isEducationCompleted BIT = 0,
    @isExperienceCompleted BIT = 0,
    @oldCompletedSteps INT = NULL,
    @newCompletedSteps INT = NULL;

SELECT 
-- oldCompletedSteps
    @oldCompletedSteps = [Contact].[OCASLR_CompletedSteps]
-- isPersonalInformationCompleted
    , @isPersonalInformationCompleted =
    CASE WHEN 
        [Contact].[FirstName] IS NOT NULL AND
        [Contact].[LastName] IS NOT NULL AND
        [Contact].[BirthDate] IS NOT NULL AND
        [Contact].[ocaslr_gendercodeid] IS NOT NULL AND
        [Contact].[ocaslr_firstlanguageid] IS NOT NULL AND
        [Contact].[ocaslr_firstgenerationapplicantid] IS NOT NULL
    THEN 1
    ELSE 0
    END
-- isContactInformationCompleted
    , @isContactInformationCompleted =
    CASE WHEN
        (
            [Address].[Line1] IS NOT NULL OR
            [Address].[Line2] IS NOT NULL
        ) AND
        [Address].[City] IS NOT NULL AND
        [Address].[Country] IS NOT NULL AND
        (
            (
                [Address].[Country] IS NOT NULL AND
                [Address].[Country] <> 'Canada'
            ) OR
            [Address].[Country] = 'Canada' AND
            [Address].[StateOrProvince] IS NOT NULL
        ) AND
        [Address].[PostalCode] IS NOT NULL AND
        [Contact].[Telephone2] IS NOT NULL AND
        [Contact].[EMailAddress1] IS NOT NULL AND
        [Contact].[ocaslr_preferredlanguageid] IS NOT NULL AND
        [Contact].[ocaslr_preferredcorrespondencemethodid] IS NOT NULL AND
        [Contact].[ocaslr_preferredsponsoragencyid] IS NOT NULL
    THEN 1
    ELSE 0
    END
-- isCitizenshipAndResidencyCompleted
    , @isCitizenshipAndResidencyCompleted =
    CASE WHEN
        [Contact].[ocaslr_countryofcitizenshipid] IS NOT NULL AND
        [Contact].[ocaslr_statusincanadaid] IS NOT NULL AND
        [Contact].[ocaslr_countryofbirthid] IS NOT NULL AND
        [Contact].[ocaslr_IsAboriginalPerson] IS NOT NULL AND
        (
            [Contact].[ocaslr_IsAboriginalPerson] = 0 OR
            (
                (
                    [Contact].[ocaslr_IsAboriginalPerson] = 1 AND
                    [Contact].[ocaslr_aboriginalstatusid] IS NOT NULL
                ) OR
                (
                    [Contact].[ocaslr_IsAboriginalPerson] = 1 AND
                    [Contact].[ocaslr_otheraboriginalstatus] IS NOT NULL AND
                    [OtherAboriginalStatus].[ocaslr_aboriginalstatusId] IS NOT NULL
                )
            )
       )
    THEN 1
    ELSE 0
    END
-- isEducationCompleted
    , @isEducationCompleted =
    CASE WHEN
        [Contact].[ocaslr_highschoolenrolled] IS NOT NULL AND
        (
            (
                [Contact].ocaslr_highschoolenrolled = 1 AND
                [Contact].ocaslr_highschoolgraduationdate IS NOT NULL
            ) OR
            (
                [Contact].ocaslr_highschoolenrolled = 0 AND
                [Contact].ocaslr_highschoolgraduated IS NOT NULL
            )
        ) AND
        [Education].[ocaslr_applicantid] IS NOT NULL
    THEN 1
    ELSE 0
    END
-- isExperienceCompleted
    , @isExperienceCompleted =
    CASE WHEN
        [Education].[ocaslr_applicantid] IS NOT NULL
    THEN 1
    ELSE 0
    END
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ContactBase] [Contact]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[CustomerAddressBase] As [Address] ON ([Contact].[ContactId] = [Address].ParentId AND [Address].AddressNumber = 1)
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_educationBase] [Education] ON [Contact].[ContactId] = [Education].[ocaslr_applicantid]
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_aboriginalstatusBase] [OtherAboriginalStatus] ON [Contact].[ocaslr_aboriginalstatusid] = [OtherAboriginalStatus].[ocaslr_aboriginalstatusId]
        AND RIGHT([OtherAboriginalStatus].[ocaslr_coltranecode], 1) = '1'
WHERE [Contact].[ContactId] = @Id AND
    [Contact].[StateCode] = 0 AND
    [Contact].[StatusCode] = 1;


IF (@isPersonalInformationCompleted = 1 AND @isContactInformationCompleted = 1 AND @isCitizenshipAndResidencyCompleted = 1 AND @isEducationCompleted = 1 AND @isExperienceCompleted = 1)
    SET @newCompletedSteps = 7;
ELSE IF (@isPersonalInformationCompleted = 1 AND @isContactInformationCompleted = 1 AND @isCitizenshipAndResidencyCompleted = 1 AND @isEducationCompleted = 1)
    SET @newCompletedSteps = 5;
ELSE IF (@isPersonalInformationCompleted = 1 AND @isContactInformationCompleted = 1 AND @isCitizenshipAndResidencyCompleted = 1)
    SET @newCompletedSteps = 4;
ELSE IF (@isPersonalInformationCompleted = 1 AND @isContactInformationCompleted = 1)
    SET @newCompletedSteps = 2;
ELSE IF (@isPersonalInformationCompleted = 1)
    SET @newCompletedSteps = 1;

IF ISNULL(@newCompletedSteps, -1) <> ISNULL(@oldCompletedSteps, -1)
    SELECT 1 AS [HasChanged], @newCompletedSteps AS [NewCompletedSteps]
ELSE
    SELECT 0 AS [HasChanged], @newCompletedSteps AS [NewCompletedSteps]
