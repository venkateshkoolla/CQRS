SELECT college.ocaslr_collegecode AS [CollegeCode], count(*) AS [Count]
                FROM dbo.ocaslr_programbase program WITH (nolock)
                LEFT JOIN dbo.accountbase campus WITH (nolock)
                        ON program.ocaslr_campusid = campus.accountid
                LEFT JOIN customeraddressbase campusaddress WITH (nolock)
                        ON (campus.accountid = campusaddress.parentid AND campusaddress.addressnumber = 1)
                LEFT JOIN dbo.accountbase campusbase WITH (nolock)
                        ON campus.accountid = campusbase.accountid
                LEFT JOIN dbo.accountbase college WITH (nolock)
                        ON campusbase.parentaccountid = college.accountid
                LEFT JOIN dbo.accountbase collegebase WITH (nolock)
                        ON college.accountid = collegebase.accountid
                LEFT JOIN dbo.ocaslr_collegeinformationbase collegeinfomation WITH (nolock)
                        ON college.accountid = collegeinfomation.ocaslr_collegeid
                LEFT JOIN dbo.ocaslr_collegeapplicationcyclebase collappcycle WITH (nolock)
                        ON program.ocaslr_collegeapplicationcycleid = collappcycle.ocaslr_collegeapplicationcycleid
                LEFT JOIN dbo.ocaslr_applicationcyclebase appcycle WITH (nolock)
                        ON collappcycle.ocaslr_applicationcycleid = appcycle.ocaslr_applicationcycleid
                LEFT JOIN dbo.ocaslr_applicationcyclestatusbase appcycleststus WITH (nolock)
                        ON appcycle.ocaslr_applicationcyclestatusid = appcycleststus.ocaslr_applicationcyclestatusid
                LEFT JOIN dbo.ocaslr_credentialbase credential WITH (nolock)
                        ON program.ocaslr_credentialid = credential.ocaslr_credentialid
                LEFT JOIN dbo.ocaslr_programlevelbase programlevel WITH (nolock)
                        ON program.ocaslr_programlevelid = programlevel.ocaslr_programlevelid
                LEFT JOIN dbo.ocaslr_programtypebase TYPE WITH (nolock)
                        ON program.ocaslr_programtypeid = TYPE.ocaslr_programtypeid
                LEFT JOIN dbo.ocaslr_entrylevelbase entrylevel WITH (nolock)
                        ON program.ocaslr_entrylevelid = entrylevel.ocaslr_entrylevelid
                LEFT JOIN dbo.ocaslr_unitofmeasurebase unit WITH (nolock)
                        ON program.ocaslr_unitofmeasureid = unit.ocaslr_unitofmeasureid
                LEFT JOIN dbo.ocaslr_programlanguagebase programlanguage WITH (nolock)
                        ON program.ocaslr_programlanguageid = programlanguage.ocaslr_programlanguageid
                LEFT JOIN dbo.ocaslr_mcucodebase mcu WITH (nolock)
                        ON program.ocaslr_mcucodeid = mcu.ocaslr_mcucodeid
                LEFT JOIN dbo.ocaslr_highlycompetitivebase higlycompetitive WITH (nolock)
                        ON program.ocaslr_highlycompetitiveid = higlycompetitive.ocaslr_highlycompetitiveid
                LEFT JOIN dbo.ocaslr_ministryapprovalbase approval WITH (nolock)
                        ON program.ocaslr_ministryapprovalid = approval.ocaslr_ministryapprovalid
                LEFT JOIN dbo.ocaslr_promotionbase promotion WITH (nolock)
                        ON program.ocaslr_promotionid = promotion.ocaslr_promotionid
                LEFT JOIN dbo.ocaslr_programspecialcodebase specialcode WITH (nolock)
                        ON program.ocaslr_specialcodeid = specialcode.ocaslr_programspecialcodeid
                LEFT JOIN dbo.ocaslr_offerstudymethodBase programdelivery WITH (nolock)
                        ON programdelivery.ocaslr_offerstudymethodId = program.ocaslr_ProgramDelivery
                LEFT JOIN dbo.ocaslr_programcategoryBase category1 WITH (nolock)
                        ON program.ocaslr_programcategory1id = category1.ocaslr_programcategoryId
                LEFT JOIN dbo.ocaslr_programcategoryBase category2 WITH (nolock)
                        ON program.ocaslr_programcategory2id = category2.ocaslr_programcategoryId
                LEFT JOIN dbo.ocaslr_programsubcategoryBase subcategory1 WITH (nolock)
                        ON program.ocaslr_subcategory1id = subcategory1.ocaslr_programsubcategoryId
                LEFT JOIN dbo.ocaslr_programsubcategoryBase subcategory2 WITH (nolock)
                        ON program.ocaslr_programsubcategory2id = subcategory2.ocaslr_programsubcategoryId
                LEFT JOIN ocaslr_programintakeBase intake WITH (nolock)
                        ON intake.ocaslr_programid = program.ocaslr_programid
                LEFT JOIN ocaslr_programintakestatusBase intakestatus WITH (nolock)
                        ON intake.ocaslr_programintakestatusid = intakestatus.ocaslr_programintakestatusid
                LEFT JOIN ocaslr_programintakeavailabilityBase intakeavailable WITH (nolock)
                        ON intake.ocaslr_availabilitytid = intakeavailable.ocaslr_programintakeavailabilityId
                LEFT JOIN dbo.ocaslr_entrylevelbase intakeEntryLevel WITH (nolock)
                        ON intake.ocaslr_defaultentrysemester = intakeEntryLevel.ocaslr_entrylevelid
                WHERE appcycleststus.ocaslr_code = 'A'
                    AND intakestatus.ocaslr_code ='A'
                    AND college.accountid IS NOT NULL
                    AND intake.statecode = 0 AND intake.statuscode = 1
                    AND program.statecode = 0 AND program.statuscode = 1
                    AND CAST(RIGHT(intake.Ocaslr_startdate,2) + '/1/' + LEFT(intake.Ocaslr_startdate,2) AS DATE) > DATEADD(mm,-3,GETDATE())
                    AND appcycle.ocaslr_applicationcycleid = @ApplicationCycleId
                    AND CASE WHEN intakeavailable.ocaslr_code <> 'C' AND intakestatus.ocaslr_code = 'A' AND program.statecode = 0 AND promotion.ocaslr_code = '01' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END = @IntakeCanApply
                GROUP BY college.ocaslr_collegecode