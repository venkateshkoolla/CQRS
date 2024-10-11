CREATE VIEW [dbo].[view_ProgramIntakes]
	AS
SELECT   [ocaslr_programintakeId] AS [Id]
        ,[Intake].[ocaslr_programid] AS [ProgramId]
        ,[Program].[ocaslr_collegeapplicationcycleid] AS [CollegeApplicationCycleId]
        ,[CollegeApplicationCycle].[ocaslr_applicationcycleid] AS [ApplicationCycleId]
        ,[Program].[ocaslr_promotionid] AS [PromotionId]
        ,[Program].[ocaslr_programcode] AS [ProgramCode]
        ,[Program].[ocaslr_title] AS [ProgramTitle]
        ,[Program].[ocaslr_ProgramDelivery] AS [ProgramDeliveryId]
        ,[Program].[ocaslr_campusid] AS [CampusId]
        ,[Intake].[ocaslr_availabilitytid] AS [AvailabilityId]
        ,[Intake].[ocaslr_name] AS [Name]
        ,[Intake].[ocaslr_expirydate] AS [ExpiryDate]
        ,[Intake].[ocaslr_enrolmentprojection] AS [EnrolmentProjection]
        ,[Intake].[ocaslr_enrolmentmaximum] AS [EnrolmentMaximum]
        ,[Intake].[ocaslr_expiryactionid] AS [ExpiryActionId]
        ,[Intake].[ocaslr_programintakestatusid] AS [ProgramIntakeStatusId]
        ,CASE WHEN ISNULL([Intake].[ocaslr_overrideentrysemesters], 0) = 0 THEN
            [Program].[ocaslr_entrylevelid]
        ELSE
            [Intake].[ocaslr_defaultentrysemester]
        END AS [DefaultEntrySemesterId]
        ,[Intake].[CreatedOn]
        ,[Intake].[ModifiedOn]
        ,[Intake].ocaslr_modifiedbyuser AS ModifiedBy
        ,[Intake].[statecode] AS [State]
        ,[Intake].[statuscode] AS [Status]
        ,[Intake].[ocaslr_overrideentrysemesters] AS [HasSemesterOverride]
        ,[Intake].[Ocaslr_startdate] AS [StartDate]
        ,[CollegeApplicationCycle].[ocaslr_collegeid] AS CollegeId
        ,[Intake].[statecode] AS StateCode
        ,[Intake].[statuscode] AS StatusCode
        -- the following fields are mapped to IList<Guid> EntryLevelIds
        ,CASE WHEN ISNULL([Intake].[ocaslr_overrideentrysemesters], 0) = 0 THEN
            [EntryLevel].[ocaslr_entrylevelId]
         ELSE
            [EntrySemester].[ocaslr_entrylevelId]
         END AS [EntryLevelId]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programintakeBase] [Intake]
    JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programBase] [Program] ON [Program].ocaslr_programId = [Intake].ocaslr_programid
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_collegeapplicationcycleBase] [CollegeApplicationCycle] ON [Program].ocaslr_collegeapplicationcycleid = [CollegeApplicationCycle].ocaslr_collegeapplicationcycleId
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_program_entrylevelsBase] [ProgramEntryLevel] ON [Intake].[ocaslr_programid] = [ProgramEntryLevel].[ocaslr_programid] AND ISNULL([Intake].ocaslr_overrideentrysemesters,0) = 0
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_entrylevelBase] [EntryLevel] ON [EntryLevel].ocaslr_entrylevelId = [ProgramEntryLevel].ocaslr_entrylevelid AND ISNULL([Intake].ocaslr_overrideentrysemesters,0) = 0
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_programintakeentrysemesterBase] [IntakeEntrySemester] ON [IntakeEntrySemester].ocaslr_programintake = [Intake].ocaslr_programintakeId AND ISNULL([Intake].ocaslr_overrideentrysemesters,0) = 1
    LEFT OUTER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_entrylevelBase] [EntrySemester] ON [EntrySemester].ocaslr_entrylevelId = [IntakeEntrySemester].ocaslr_entrysemester AND ISNULL([Intake].ocaslr_overrideentrysemesters,0) = 1

