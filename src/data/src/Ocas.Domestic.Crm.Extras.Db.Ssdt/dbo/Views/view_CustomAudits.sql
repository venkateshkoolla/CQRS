CREATE VIEW [dbo].[view_CustomAudits]
    AS
SELECT   [CustomAudit].[custom_auditId] AS [Id]
       , [CustomAudit].[ocaslr_contactid] AS [ApplicantId]
       , [CustomAudit].[ocaslr_applicationid] AS [ApplicationId]
       , [CustomAudit].[CreatedOn]
       , [CustomAudit].[ModifiedOn]
       , [CustomAudit].[ocaslr_orderid] AS [OrderId]
       , [CustomAudit].[custom_entity_label] AS [CustomEntityLabelEnglish]
       , [CustomAudit].[Ocaslr_entity_label_fr] AS [CustomEntityLabelFrench]
       , [CustomAudit].[Ocaslr_modifiedbyuser] AS ModifiedBy
       , [CustomAudit].[custom_message] AS CustomMessage
       , [CustomAudit].[Ocaslr_audittype] AS CustomAuditType
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[custom_auditBase] [CustomAudit]
WHERE [CustomAudit].[statecode] = 0