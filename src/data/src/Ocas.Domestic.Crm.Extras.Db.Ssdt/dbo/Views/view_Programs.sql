CREATE VIEW [dbo].[view_Programs]
	AS
SELECT
    PRO.ocaslr_programId AS Id
    , PRO.ocaslr_collegeapplicationcycleid AS CollegeApplicationCycleId
    , CAC.ocaslr_collegeid AS CollegeId
    , CAC.ocaslr_applicationcycleid AS ApplicationCycleId
    , ocaslr_campusid  AS CampusId
    , PRO.ocaslr_name  AS [Name]
    , ocaslr_programcode  AS Code
    , ocaslr_title  As Title
    , ocaslr_ProgramDelivery  AS DeliveryId
    , ocaslr_programtypeid AS ProgramTypeId
    , ocaslr_length AS Length
    , ocaslr_unitofmeasureid AS LengthTypeId
    , ocaslr_mcucodeid AS McuCodeId
    , ocaslr_credentialid AS CredentialId
    , PRO.ocaslr_entrylevelid AS DefaultEntryLevelId
    , ocaslr_studyareaid AS StudyAreaId
    , ocaslr_highlycompetitiveid AS HighlyCompetitiveId
    , ocaslr_programlanguageid AS LanguageId
    , ocaslr_programlevelid AS LevelId
    , ocaslr_promotionid AS PromotionId
    , ocaslr_adulttrainingid AS AdultTrainingId
    , ocaslr_specialcodeid AS SpecialCodeId
    , ocaslr_apsnumber AS ApsNumber
    , ocaslr_ministryapprovalid AS MinistryApprovalId
    , ocaslr_url AS [Url]
    , ocaslr_programcategory1id AS ProgramCategory1Id
    , ocaslr_subcategory1id AS ProgramSubCategory1Id
    , ocaslr_programcategory2id AS ProgramCategory2Id
    , ocaslr_programsubcategory2id AS ProgramSubCategory2Id
    , PRO.CreatedOn
    , PRO.ModifiedOn
    , PRO.ocaslr_modifiedbyuser AS ModifiedBy
    , PRO.[statecode] AS StateCode
    , PEL.ocaslr_entrylevelid AS EntryLevelId
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programBase] AS PRO
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_collegeapplicationcycleBase] AS CAC ON PRO.[ocaslr_collegeapplicationcycleid] = CAC.ocaslr_collegeapplicationcycleId
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_program_entrylevelsBase] PEL ON PRO.[ocaslr_programId] = PEL.[ocaslr_programid]