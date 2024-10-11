SELECT 
                collappcycle.ocaslr_collegeid [CollegeId],
                college.ocaslr_collegecode [CollegeCode],
                college.name [AlternateCollegeName],
                program.ocaslr_campusid [CampusId],
                appcycle.ocaslr_applicationcycleid [ApplicationCycleId],
                program.ocaslr_programId [ProgramId],
                program.ocaslr_programcode [ProgramCode],
                programdelivery.ocaslr_offerstudymethodId [ProgramDeliveryId],
                intake.ocaslr_programintakeId [IntakeId],
                intake.ocaslr_overrideentrysemesters [HasSemesterOverride],
                intake.Ocaslr_startdate [IntakeStartDate],
                CASE WHEN intakeavailable.ocaslr_code <> 'C' AND intakestatus.ocaslr_code = 'A' AND program.statecode = 0 AND promotion.ocaslr_code = '01' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS [IntakeCanApply],
                CASE WHEN ISNULL(intake.[ocaslr_overrideentrysemesters], 0) = 0 THEN
                   program.ocaslr_entrylevelid
                ELSE
                   intake.ocaslr_defaultentrysemester
                END AS [ProgramEntryLevelId],
                CASE WHEN ISNULL(intake.[ocaslr_overrideentrysemesters], 0) = 0 THEN
                   programValidEntryLevel.ocaslr_entrylevelId
                ELSE
                   intakeValidEntryLevel.ocaslr_entrysemester
                END AS [EntryLevelId]
                FROM dbo.ocaslr_programbase program WITH (nolock)
                INNER JOIN dbo.ocaslr_collegeapplicationcyclebase collappcycle WITH (nolock)
                        ON program.ocaslr_collegeapplicationcycleid = collappcycle.ocaslr_collegeapplicationcycleid
                INNER JOIN dbo.AccountBase college WITH (nolock)
                        ON collappcycle.ocaslr_collegeid = college.AccountId
                INNER JOIN dbo.ocaslr_applicationcyclebase appcycle WITH (nolock)
                        ON collappcycle.ocaslr_applicationcycleid = appcycle.ocaslr_applicationcycleid
                INNER JOIN dbo.ocaslr_applicationcyclestatusbase appcycleststus WITH (nolock)
                        ON appcycle.ocaslr_applicationcyclestatusid = appcycleststus.ocaslr_applicationcyclestatusid
                INNER JOIN dbo.ocaslr_promotionbase promotion WITH (nolock)
                        ON program.ocaslr_promotionid = promotion.ocaslr_promotionid
                INNER JOIN dbo.ocaslr_offerstudymethodBase programdelivery WITH (nolock)
                        ON programdelivery.ocaslr_offerstudymethodId = program.ocaslr_ProgramDelivery
                INNER JOIN ocaslr_programintakeBase intake WITH (nolock)
                        ON intake.ocaslr_programid = program.ocaslr_programid
                INNER JOIN ocaslr_programintakestatusBase intakestatus WITH (nolock)
                        ON intake.ocaslr_programintakestatusid = intakestatus.ocaslr_programintakestatusid
                INNER JOIN ocaslr_programintakeavailabilityBase intakeavailable WITH (nolock)
                        ON intake.ocaslr_availabilitytid = intakeavailable.ocaslr_programintakeavailabilityId
                LEFT JOIN dbo.ocaslr_program_entrylevelsBase programValidEntryLevel WITH (nolock)
                        ON ISNULL([Intake].ocaslr_overrideentrysemesters,0) = 0 AND program.ocaslr_programId = programValidEntryLevel.ocaslr_programid
                LEFT JOIN dbo.ocaslr_programintakeentrysemesterBase intakeValidEntryLevel WITH (nolock)
                        ON ISNULL([Intake].ocaslr_overrideentrysemesters,0) = 1 AND intake.ocaslr_programintakeId = intakeValidEntryLevel.ocaslr_programintake
                WHERE appcycleststus.ocaslr_code = 'A'
                    AND intakestatus.ocaslr_code ='A'
                    AND college.accountid IS NOT NULL
                    AND intake.statecode = 0
                    AND program.statecode = 0
                    AND appcycle.ocaslr_applicationcycleid = @ApplicationCycleId
                    AND collappcycle.ocaslr_collegeid = @CollegeId
                    AND CASE WHEN intakeavailable.ocaslr_code <> 'C' AND intakestatus.ocaslr_code = 'A' AND program.statecode = 0 AND promotion.ocaslr_code = '01' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END = 1