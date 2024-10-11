CREATE VIEW [dbo].[view_ProgramApplications]
	AS
SELECT APP.Id AS ApplicationId
    , APP.ApplicationNumber
    , APP.ApplicationStatusId
    , CON.FirstName AS ApplicantFirstName
    , CON.LastName AS ApplicantLastName
    , PC.ProgramId
    , PC.[ProgramIntakeId] AS IntakeId
    , PC.statecode AS StateCode 
FROM [dbo].[view_ProgramChoices] PC
    INNER JOIN  [dbo].[view_Applications] APP ON PC.ApplicationId = APP.Id
    INNER JOIN  [dbo].[view_Contacts] CON ON CON.Id = APP.ApplicantId