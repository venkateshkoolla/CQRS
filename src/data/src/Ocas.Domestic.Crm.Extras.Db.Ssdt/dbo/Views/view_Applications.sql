CREATE VIEW [dbo].[view_Applications]
    AS
SELECT [ocaslr_applicationId] AS Id
      ,[ocaslr_applicationstatusid] AS ApplicationStatusId
      ,[ocaslr_effectivedate] AS EffectiveDate
      ,[ocaslr_balance_Base] AS BalanceBase
      ,[ocaslr_basisforadmissionlock] AS BasisForAdmissionLock
      ,[ocaslr_applicantid] AS ApplicantId
      ,APP.[ocaslr_applicationcycleid] AS ApplicationCycleId
      ,[Ocaslr_eventtrigger] AS EventTrigger
      ,[ocaslr_currentlock] AS CurrentLock
      ,[ocaslr_shoppingcartstatus] AS ShoppingCartStatus
      ,APP.[ocaslr_name] AS [Name]
      ,[ocaslr_applicationnumber] AS ApplicationNumber
      ,[ocaslr_currentid] AS CurrentId
      ,[ocaslr_basisforadmissionid] AS BasisForAdmissionId
      ,[ocaslr_balance] AS Balance
      ,[ocaslr_completedsteps] AS CompletedSteps
      ,APP.[CreatedOn]
      ,APP.[ModifiedOn]
      ,[ocaslr_modifiedbyuser] AS ModifiedBy
      ,APP.[statecode] AS StateCode
      ,APP.[statuscode] AS StatusCode
      ,ACS.[ocaslr_code] AS Code
  FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationBase] AS APP
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcycleBase] AS AC ON AC.[ocaslr_applicationcycleId] = APP.[ocaslr_applicationcycleid]
    INNER JOIN [$(SERVER)].[OCAS_MSCRM].[dbo].[ocaslr_applicationcyclestatusBase] AS ACS ON AC.[ocaslr_applicationcyclestatusid] = ACS.[ocaslr_applicationcyclestatusId]
