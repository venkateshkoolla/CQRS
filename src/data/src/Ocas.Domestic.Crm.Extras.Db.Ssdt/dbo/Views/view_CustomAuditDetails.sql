CREATE VIEW [dbo].[view_CustomAuditDetails]
    AS
SELECT [CustomAuditDetail].[custom_audit_detailId] AS [Id]
       , [CustomAuditDetail].[custom_auditid] AS [CustomAuditId]
       , [CustomAuditDetail].[custom_display_name] AS [DisplayName]
       , [CustomAuditDetail].[custom_original_value] AS [OriginalValue]
       , [CustomAuditDetail].[custom_new_value] AS [NewValue]
       , [CustomAuditDetail].[custom_schema_name] AS [SchemaName]
FROM [$(SERVER)].[OCAS_MSCRM].[dbo].[custom_audit_detailBase] [CustomAuditDetail]
WHERE [CustomAuditDetail].[statecode] = 0